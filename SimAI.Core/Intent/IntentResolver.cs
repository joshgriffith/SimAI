using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimAI.Core.OpenAI;
using SimAI.Core.Runtime;
using SimAI.Core.Skills;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Intent {
    public class IntentResolver : IsQueryHandler {
        public List<string> Entities { get; set; } = new();
        public DateTime Time { get; set; }

        private readonly OpenAIProvider _provider;
        private readonly SkillProvider _skills;
        private readonly IsTokenizer _tokenizer;
        private readonly IntentSummarizer _summarizer;

        public IntentResolver(OpenAIProvider provider, SkillProvider skills, IsTokenizer tokenizer, IntentSummarizer summarizer) {
            Time = DateTime.Now;
            _provider = provider;
            _skills = skills;
            _tokenizer = tokenizer;
            _summarizer = summarizer;
        }

        public async Task Handle(QueryContext query) {

            // Steps:
            // 1.  Check if we have handled this query before
            // 2.  Check for an exact intent match
            // 3.  Ask OpenAI for summary

            //var summary = await _summarizer.GetIntentAsync(query.Request);
            var response = await Handle(query.Request);

            if (response != null) {
                if (response is string or int or DateTime)
                    query.Response = response.ToString();
                else
                    query.Response = JsonConvert.SerializeObject(response);
            }
        }

        public async Task<object> Handle(string query) {
            var sequence = await _tokenizer.Tokenize(query);

            while (true) {
                var range = sequence.GetNextSegment();

                if (range.Start.Value == 0 && range.End.Value == sequence.Size)
                    return await GetResponse(sequence);
                
                var tokens = sequence.Tokens
                    .Skip(range.Start.Value + 1)
                    .Take(range.End.Value - range.Start.Value)
                    .Where(each => each.Value != "(" && each.Value != ")")
                    .ToArray();

                var response = await GetResponse(new TokenSequence(tokens));
                var token = (await _tokenizer.Tokenize(response.ToString())).Tokens.FirstOrDefault();

                sequence.Replace(range.Start.Value, range.End.Value - range.Start.Value + 1, token);
            }
        }
        
        private async Task<object> GetResponse(TokenSequence sequence) {
            object response = null;

            while (true) {
                var binding = _skills.GetIntentBinding(sequence);

                if (binding != null) {
                    var result = binding.Invoke();
                    response = result;

                    if (binding.IsPartial(sequence)) {
                        var token = (await _tokenizer.Tokenize(result.ToString())).Tokens.FirstOrDefault();
                        sequence.Replace(binding.SequenceIndex, binding.Template.Size, token);
                        continue;
                    }
                }

                break;
            }

            return response;
        }

        /*public async Task Handle(QueryContext query) {
            var request = query.Request;
            var intent = await GetIntent(request);

            if (intent.Entity == "time") {
                if (intent.Intent == "change") {
                    var functionName = request.Replace(" ", "").CamelCase();
                    var prompt = "function " + functionName + "(currentDate) {";
                    var function = await _provider.Completion(new OpenAICompletionQuery(OpenAIEngines.DavinciCodex, prompt) {
                        MaxTokens = 64,
                        Stops = new List<string> {
                            "}"
                        }
                    });

                    var code = prompt + function + "}";
                    code += Environment.NewLine;
                    code += functionName + "(new Date()).getTime();";
                    var result = InvokeJavascript(code);
                    Time = long.Tokenize(result.ToString()).FromJavascriptDate();
                }
                
                query.Response = Time.ToShortTimeString();
                return;
            }
            
            if (intent.Intent.StartsWith("create ")) {
                Entities.Add(intent.Entity);
                query.Response = intent.Entity + " appears";
                return;
            }

            if (Entities.Count > 5) {
                query.Response = "Error: Too many entities";
                return;
            }

            var output = string.Empty;
            var newState = new List<string>();

            foreach (var entity in Entities) {
                var transform = await UpdateState(entity, request);

                if (!transform.IsEmpty()) {
                    output += transform + "\n";
                    newState.Add(transform);
                }
            }

            Entities = newState.ToList();

            if (output.EndsWith("\n"))
                output = output.Substring(0, output.Length - 1);

            query.Response = output;
        }*/

        private object InvokeJavascript(string code) {
            var engine = new Jurassic.ScriptEngine();
            return engine.Evaluate(code);
        }
    }
}