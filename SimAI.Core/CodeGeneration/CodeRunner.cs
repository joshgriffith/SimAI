using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SimAI.Core.CodeGeneration {
    public class CodeRunner {

        private const string GeneratedNamespace = "DynamicCodeGeneration";
        private const string GeneratedClassName = "Runtime";
        private const string GeneratedMethodName = "Run";

        private readonly List<string> _imports = new() { "using System;", "using System.Runtime;" };

        public CodeRunner(params Type[] imports)
            : this(imports.Select(each => each.Namespace).ToArray()) {
        }

        public CodeRunner(params string[] imports) {

            foreach (var import in imports) {
                var formatted = "using " + import + ";";

                if(!_imports.Contains(formatted))
                    _imports.Add(formatted);
            }
        }

        public object InvokeMethod(string methodCode, params MethodParameterBinding[] parameters) {
            
            var compilation = Compile(methodCode, parameters);

            using var ms = new MemoryStream();

            var result = compilation.Emit(ms);

            if (!result.Success) {
                var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (var diagnostic in failures)
                    Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());

                throw new Exception(result.Diagnostics.First().ToString());
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            return InvokeGeneratedMethod(assembly, parameters);
        }

        private CSharpCompilation Compile(string methodCode, params MethodParameterBinding[] parameters) {
            var methodSignature = string.Empty;

            foreach (var parameter in parameters) {
                if (!string.IsNullOrEmpty(methodSignature))
                    methodSignature += ", ";

                methodSignature += parameter.Value.GetType().Name + " " + parameter.Name;
            }

            var imports = string.Join("\n", _imports);

            var template =
@$"{imports}

namespace {GeneratedNamespace}
{{
    public class {GeneratedClassName}
    {{
        public static string {GeneratedMethodName}({methodSignature}) {{
            {methodCode}
        }}
    }}
}}";
            var syntaxTree = CSharpSyntaxTree.ParseText(template);
            var compilation = CSharpCompilation.Create(Path.GetRandomFileName(), new[] {syntaxTree}, ReferenceAssemblies.Net50, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            return compilation;
        }

        private object InvokeGeneratedMethod(Assembly assembly, params MethodParameterBinding[] parameters) {
            var type = assembly.GetType($"{GeneratedNamespace}.{GeneratedClassName}");
            var obj = Activator.CreateInstance(type);

            return type.InvokeMember(GeneratedMethodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, parameters.Select(each => each.Value).ToArray());
        }
    }
}