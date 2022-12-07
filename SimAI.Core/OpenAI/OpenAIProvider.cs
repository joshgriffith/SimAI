using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimAI.Core.Prompts;

namespace SimAI.Core.OpenAI {
    public class OpenAIProvider {

        private readonly OpenAIClient _client;
        private readonly CosmosClient _database;
        private readonly Container _container;
        private readonly ILogger<OpenAIProvider> _logger;

        public OpenAIProvider(OpenAIClient client, CosmosClient database, ILogger<OpenAIProvider> logger) {
            _client = client;
            _database = database;
            _container = _database.GetContainer("openai", "completion");
            _logger = logger;
        }

        public async Task DeleteAll() {
            var items = await _container.GetItemLinqQueryable<TextCompletionRecord>().ToFeedIterator().ReadNextAsync();

            foreach (var item in items) {
                await _container.DeleteItemAsync<TextCompletionRecord>(item.Id, new PartitionKey(item.Partition));
            }
        }
        
        public async Task<string> Completion(OpenAICompletionQuery query, bool useCache = true) {
            
            if (query.Prompt.Length == 0 || query.MaxTokens <= 0)
                return string.Empty;
            
            //var maxSamples = (5 * query.Temperature) + 1;
            TextCompletionRecord existing = null;

            if (useCache) {
                var partition = query.GetPartitionKey();

                var sql = new QueryDefinition("SELECT * FROM completion WHERE completion.partition = @partition AND completion.query.prompt = @prompt")
                    .WithParameter("@partition", partition)
                    .WithParameter("@prompt", query.Prompt);
            
                var response = await _container.GetItemQueryIterator<TextCompletionRecord>(sql).ReadNextAsync();

                // Opportunity:  Query for query.prompt+response starts with prompt AND query.prompt is less than prompt
            
                existing = response.FirstOrDefault();
                //var estimatedTokens = prompt.Length > 4 ? prompt.Length / 4 : 1;

                if (existing != null && existing.Query.MaxTokens >= query.MaxTokens)
                    return existing.Response;
            }

            //return string.Empty;

            var result =  await _client.Completion(query);

            if (useCache) {
                var update = new TextCompletionRecord(result);

                if (existing != null)
                    update.Id = existing.Id;

                var serialized = JsonConvert.SerializeObject((object?) update, new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(serialized));
                await _container.UpsertItemStreamAsync(stream, new PartitionKey(update.Partition));
            }

            _logger?.Log(LogLevel.Warning, "OpenAI completion: " + query.Prompt);

            return result.Text;
        }

        public async Task<string> ToFunctionAsync(string prompt, params OpenAIFunctionResult[] samples) {
            if (!samples.Any(each => each.In.Length > 0))
                throw new Exception("ToFunctionAsync missing samples for prompt: " + prompt);

            /*samples = samples.Concat(new List<OpenAIFunctionResult> {
                //new() { In = "multiply 3 and 6", Out = "multiply(3, 6)" },
                new() { In = "restart the computer at 3 pm", Out = "at('3 pm').restart('computer')" },
                new() { In = "a woman is a human", Out = "is('woman', 'human')" },
                new() { In = "click submit after you sign the document", Out = "sign('document').click('submit')" },
                new() { In = "text me a list of your favorite foods", Out = "sendText('me', list('favorite foods'))" },
                new() { In = "email me a cat", Out = "email('me', findPicture('cat'))" },
                //new() { In = "i have 2 marbles in one hand and 3 marbles in the other hand, how many marbles do i have", Out = "add(2, 3)" }
            }).ToArray();*/

            var completion = new OpenAIFunctionResult {
                In = prompt
            };

            await Completion(samples.ToList(), completion);

            _logger?.Log(LogLevel.Information, "ToFunctionAsync: " + completion.Out);

            return completion.Out;
        }

        public async Task<List<string>> SegmentAsync(string prompt) {
            var samples = new List<OpenAISegmentResult> {
                new() { Prompt = "when you see a red square, email me", Segments = new List<string> {
                    new("when you see a red square"),
                    new("email me")
                } },
                new() { Prompt = "multiply 3 and 6", Segments = new List<string> {
                    new("multiply 3 and 6")
                } },
                new() { Prompt = "text me a list of your favorite foods", Segments = new List<string> {
                    new("text me"),
                    new("a list of your favorite foods")
                } },
                new() { Prompt = "count how many times I click the left mouse button over the next 5 seconds", Segments = new List<string> {
                    new("count how many times"),
                    new("I click the left mouse button"),
                    new("over the next 5 seconds")
                } }
            };

            var completion = new OpenAISegmentResult {
                Prompt = prompt
            };

            await Completion(samples, completion);

            return completion.Segments;
        }

        public async Task<string> CompleteMethod(string prompt, int maxTokens = 500, bool useCache = true, params string[] stops) {
            prompt = prompt.Replace("\t", " ").Trim().Replace("\r", "");

            var defaultStops = new List<string> { "}\n//", "}\n\n//" };

            if (stops != null && stops.Any())
                defaultStops = defaultStops.Concat(stops).ToList();

            var result = await Completion(new OpenAICompletionQuery(OpenAIEngines.TextDavinci001, prompt) {
                MaxTokens = maxTokens,
                Stops = defaultStops
            }, useCache);

            var cleaned = string.Join("", result.Split("\n").Skip(1).ToArray());

            if (cleaned.StartsWith("{"))
                cleaned = cleaned[1..];

            if (cleaned.EndsWith("}"))
                cleaned = cleaned[..^1];
            
            return cleaned.Trim();
        }

        public async Task<string> CodeCompletion(string prompt, int maxTokens = 1500, bool useCache = true) {
            return await Completion(new OpenAICompletionQuery(OpenAIEngines.TextDavinci001, prompt.Replace("\r", "")) {
                MaxTokens = maxTokens
            }, useCache);
        }

        public async Task<T> Completion<T>(List<T> samples, T toComplete = default, OpenAIEngines engine = OpenAIEngines.TextDavinci001, int maxTokens = 64) where T : new() {
            
            var prompt = SerializedPrompt.FromSamples(samples, toComplete);
            
            var result = await Completion(new OpenAICompletionQuery(engine, prompt.Body) {
                MaxTokens = maxTokens,
                Stops = new List<string> {
                    //"\n",
                    "\"\r",
                    "\" }"
                }
            }, false);

            var json = prompt.Fragment + result.Substring(0, result.IndexOf("}") + 1);
            JsonConvert.PopulateObject(json, toComplete);
            return toComplete;
        }
    }
}