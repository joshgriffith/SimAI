using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimAI.Core.Extensions;

namespace SimAI.Core.TextToSpeech {
    public class UberduckClient : IsTextToSpeechProvider, IDisposable {

        public TimeSpan Timeout = new(0, 0, 0, 30);
        private const string _baseUrl = "https://api.uberduck.ai/";
        private readonly HttpClient _client;

        public UberduckClient() : this("pub_wfnzredvzmseqsgkpz", "pk_ae06a3d1-af7f-4423-a83f-348fdbee8ea3") {
        }

        public UberduckClient(string key, string secret) {
            _client = new HttpClient();

            var byteArray = new UTF8Encoding().GetBytes(key + ":" + secret);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public void Dispose() {
            _client?.Dispose();
        }

        public async Task<Stream> GetStreamAsync(string speech, string voice = "glados") {
            voice = voice.Replace(" ", "-").ToLower();

            var source = new CancellationTokenSource(Timeout);
            const string url = _baseUrl + "speak";

            using var response = await _client.PostAsJsonAsync(url, new {
                speech,
                voice
            }, source.Token);
            
            if (!source.IsCancellationRequested) {
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<SpeechJobModel>(json);

                while (!source.IsCancellationRequested) {
                    var status = await GetStatusAsync(model.UUID);

                    if (status.FailedAt.HasValue)
                        throw new Exception("Failed to generate TextToSpeech");

                    if (status.FinishedAt.HasValue && !string.IsNullOrEmpty(status.Path)) {
                        var client = new HttpClient();
                        var audioPath = await client.GetAsync(status.Path, HttpCompletionOption.ResponseContentRead);
                        return await audioPath.Content.ReadAsStreamAsync();
                    }

                    Thread.Sleep(2500);
                }
            }

            return null;
        }

        public async Task<string> DownloadAsync(string speech, string voice = "glados") {
            var stream = await GetStreamAsync(speech, voice);

            if (stream == null)
                throw new Exception("Did not receive response.");

            var path = Path.GetTempFileName();

            await using var file = File.OpenWrite(path);
            await stream.CopyToAsync(file);


            return path;
        }

        private async Task<SpeechStatusModel> GetStatusAsync(string uuid) {
            var url = _baseUrl + "speak-status?uuid=" + uuid;
            var json = await _client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<SpeechStatusModel>(json);
        }

        protected class SpeechJobModel {
            public string UUID { get; set; }
        }

        protected class SpeechStatusModel {

            [JsonProperty("started_at")]
            public DateTime? StartedAt { get; set; }

            [JsonProperty("failed_at")]
            public DateTime? FailedAt { get; set; }

            [JsonProperty("finished_at")]
            public DateTime? FinishedAt { get; set; }
            
            public string Path { get; set; }
        }
    }
}