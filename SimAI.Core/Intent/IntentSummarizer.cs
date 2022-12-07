using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimAI.Core.OpenAI;

namespace SimAI.Core.Intent {
    public class IntentSummarizer {
        private readonly OpenAIProvider _provider;

        public IntentSummarizer(OpenAIProvider provider) {
            _provider = provider;
        }
        
        public async Task<string> GetIntentAsync(string prompt) {

            var samples = new List<IntentPromptResult> {
                new() { Prompt = "what time is it?", Intent = "get time" },
                new() { Prompt = "what is 2 plus 2", Intent = "add" },
                new() { Prompt = @"what files are in c:\foo\bar", Intent = "get files" },
                new() { Prompt = "33 * 5", Intent = "multiply" },
                new() { Prompt = "click the left mouse button 3 times", Intent = "click mouse" }
            };

            var completion = new IntentPromptResult {
                Prompt = prompt
            };

            await _provider.Completion(samples, completion);
            return completion.Intent;
        }
    }
}
