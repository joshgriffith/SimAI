using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimAI.Core.OpenAI {
    public class OpenAICompletionQuery {
        
        public string Prompt { get; set; }

        public int MaxTokens { get; set; } = 16;

        public decimal Temperature { get; set; } = 0;

        //public decimal Top { get; set; } = 1m;

        public OpenAIEngines Engine { get; set; }

        public List<string> Stops { get; set; } = new();

        public decimal PresencePenalty { get; set; } = 0;

        public decimal FrequencyPenalty { get; set; } = 0;

        public OpenAICompletionQuery(OpenAIEngines engine, string prompt) {
            Engine = engine;
            Prompt = prompt;
        }

        public object ToRequestBody() {
            if (Stops.Any()) {
                return new {
                    prompt = Prompt,
                    max_tokens = MaxTokens,
                    temperature = Temperature,
                    top_p = 1,
                    stop = Stops.ToArray(),
                    presence_penalty = PresencePenalty,
                    frequency_penalty = FrequencyPenalty
                };
            }

            return new {
                prompt = Prompt,
                max_tokens = MaxTokens,
                temperature = Temperature,
                top_p = 1,
                presence_penalty = PresencePenalty,
                frequency_penalty = FrequencyPenalty
            };
        }

        public string GetPartitionKey() {
            var key = OpenAIHelper.GetEngineName(Engine) + ":" + Temperature + ":" + PresencePenalty + ":" + FrequencyPenalty;

            if (Stops.Any())
                key += ":" + string.Join("_", Stops);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(key));
        }
    }
}