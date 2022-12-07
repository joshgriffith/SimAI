using System.Threading.Tasks;
using SimAI.Core.CodeGeneration;
using SimAI.Core.OpenAI;

namespace SimAI.Knowledge.CodeGeneration {
    public class StringTransformationQuery {
        private readonly OpenAIProvider _provider;
        
        public StringTransformationQuery(OpenAIProvider provider) {
            _provider = provider;
        }

        public async Task<string> Prompt(string inputText, string query) {
            
            var methodName = await new MethodNameGenerator(_provider).Prompt(query);
            
            var prompt =
$@"// {query}
public static string " + methodName + "(string input) {";
            
            var method = await _provider.CompleteMethod(prompt);
            var runner = new CodeRunner(typeof(System.Text.RegularExpressions.Regex));
            var result = runner.InvokeMethod(method, new MethodParameterBinding("input", inputText));

            return result != null ? result.ToString().Trim() : string.Empty;
        }
    }
}