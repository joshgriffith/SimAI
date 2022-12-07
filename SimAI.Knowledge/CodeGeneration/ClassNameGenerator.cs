using System.Collections.Generic;
using System.Threading.Tasks;
using SimAI.Core.OpenAI;
using SimAI.Core.Prompts;

namespace SimAI.Knowledge.CodeGeneration {
    public class ClassNameGenerator {
        private readonly OpenAIProvider _provider;

        public ClassNameGenerator(OpenAIProvider provider) {
            _provider = provider;
        }

        public async Task<string> Prompt(string query) {

            var samples = new List<BasePrompt> {
                new ("tasks should be awaited", "AsyncTasksShouldBeAwaitedAnalyzer"),
                new ("Identifiers should be spelled correctly", "IdentifiersShouldBeSpelledCorrectlyAnalyzer"),
            };
            
            var result = await _provider.Completion(samples, new BasePrompt(query));
            return result.Out;
        }
    }
}