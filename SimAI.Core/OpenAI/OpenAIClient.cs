using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimAI.Core.Extensions;

namespace SimAI.Core.OpenAI {
    public class OpenAIClient : IDisposable {
        private const string _baseUrl = "https://api.openai.com/v1/";
        private readonly HttpClient _client;

        public OpenAIClient(string apiKey) {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public void Dispose() {
            _client?.Dispose();
        }

        public async Task<string> GetEnginesAsync() {
            var response = await _client.GetAsync(_baseUrl + "engines");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<OpenAICompletionResult> Completion(OpenAICompletionQuery query) {
            var engineName = OpenAIHelper.GetEngineName(query.Engine);
            var url = _baseUrl + $"engines/{engineName}/completions";
            var queryTime = DateTime.UtcNow;

            var body = query.ToRequestBody();
            var response = await _client.PostAsJsonAsync(url, body, CancellationToken.None);
            var json = await response.Content.ReadAsStringAsync();
            var responseTime = DateTime.UtcNow;
            var result = JsonConvert.DeserializeObject<OpenAICompletionResponse>(json);
            var latency = (responseTime - queryTime).TotalMilliseconds;

            return new OpenAICompletionResult {
                Id = result.Id,
                QueryTime = queryTime.ToUnixTimestamp(),
                ResponseTime = responseTime.ToUnixTimestamp(),
                Latency = Convert.ToInt32(latency),
                Model = result.Model,
                Text = result.Choices[0].Text,
                FinishReason = result.Choices[0].FinishReason,
                Query = query
            };
        }
    }
}