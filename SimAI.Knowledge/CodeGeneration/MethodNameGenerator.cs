using System.Collections.Generic;
using System.Threading.Tasks;
using SimAI.Core.OpenAI;
using SimAI.Core.Prompts;

namespace SimAI.Knowledge.CodeGeneration {
    public class MethodNameGenerator {
        private readonly OpenAIProvider _provider;

        public MethodNameGenerator(OpenAIProvider provider) {
            _provider = provider;
        }

        public async Task<string> Prompt(string query) {

            var samples = new List<BasePrompt> {
                new ("finish up the task", "FinishTask"),
                new ("close the connection", "CloseConnection"),
            };
            
            var result = await _provider.Completion(samples, new BasePrompt(query));
            return result.Out;
        }
    }
}