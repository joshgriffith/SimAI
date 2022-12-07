using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimAI.Core.Extensions {
    public static class HttpClientExtensions {
        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string endpoint, object model, CancellationToken token) {
            var json = JsonConvert.SerializeObject(model);
            return await client.PostAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"), token);
        }
    }
}