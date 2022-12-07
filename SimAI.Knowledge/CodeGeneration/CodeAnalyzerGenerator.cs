using System.Threading.Tasks;
using SimAI.Core.OpenAI;

namespace SimAI.Knowledge.CodeGeneration {
    public class CodeAnalyzerGenerator {
        private readonly OpenAIProvider _provider;

        public CodeAnalyzerGenerator(OpenAIProvider provider) {
            _provider = provider;
        }

        public async Task<string> Prompt(string query) {

            var className = new ClassNameGenerator(_provider);
            var result = await className.Prompt(query);
            
            var prompt =
$@"using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

/// <summary>
/// {query}
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class {result} : DiagnosticAnalyzer";

            var code = prompt + await _provider.CodeCompletion(prompt, useCache: false, maxTokens: 2500);
            return code;
        }
    }
}