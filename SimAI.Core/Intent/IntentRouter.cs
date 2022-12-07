using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimAI.Core.Intent.Dialects;
using SimAI.Core.OpenAI;

namespace SimAI.Core.Intent {
    public class IntentRouter {

        public List<IntentMapping> Mappings = new();
        private readonly OpenAIProvider _provider;
        private readonly IsIntentDialect _dialect;
        private readonly ILogger<IntentRouter> _logger;
        
        public IntentRouter(OpenAIProvider provider, ILogger<IntentRouter> logger) {
            _provider = provider;
            _logger = logger;
            _dialect = new ImperativeIntentDialect();
        }

        public IntentRouter(OpenAIProvider provider, IsIntentDialect dialect, ILogger<IntentRouter> logger) {
            _provider = provider;
            _logger = logger;
            _dialect = dialect;
        }

        public IntentMapping<object> Use() {
            var mapping = new IntentMapping<object>(new object());
            Mappings.Add(mapping);
            return mapping;
        }

        public IntentMapping<T> Use<T>(T host) {
            var mapping = new IntentMapping<T>(host);
            Mappings.Add(mapping);
            return mapping;
        }
        
        public async Task<object> Prompt(string prompt) {
            return await Prompt(new OpenAIFunctionResult { In = prompt });
        }

        public async Task<object> Prompt<T>(T prompt) where T : IsOpenAISample, new() {
            var samples = Mappings.SelectMany(mapping => mapping.Routes).Select(route => {
                route.Sample.SetOutputValue(_dialect.Serialize(route));
                return route.Sample;
            });

            var function = await _provider.Completion(samples.Cast<T>().ToList(), prompt);
            var functions = function.GetOutputValue().Split('.');
            
            foreach (var each in functions)
                if (TryInvoke(each, out var result))
                    return result;

            return null;
        }

        private bool TryInvoke(string input, out object outcome) {
            outcome = null;

            foreach (var mapping in Mappings) {
                foreach (var route in mapping.Routes) {
                    if (input.StartsWith(route.MethodName + '(')) {
                        var parameterString = input.Substring(route.MethodName.Length + 1, input.Length - route.MethodName.Length - 2);
                        var parameters = new List<string>();
                        var parameter = string.Empty;
                        var depth = 0;

                        foreach(var token in parameterString) {
                            switch (token) {
                                case '\'':
                                    break;
                                case ',':
                                    if (depth == 0) {
                                        parameters.Add(parameter);
                                        parameter = string.Empty;
                                    }
                                    else
                                        parameter += token;

                                    break;
                                default:
                                    if (token == '(')
                                        depth += 1;
                                    else if (token == ')')
                                        depth -= 1;

                                    parameter += token;
                                    break;
                            }
                        }

                        if (parameter.Length > 0) {
                            parameters.Add(parameter);
                        }

                        parameters = parameters.Select(each => {
                            if (each.Contains("(") && each.Contains(")")) {
                                if (TryInvoke(each, out var result))
                                    return result.ToString();

                                return string.Empty;
                            }

                            return each;
                        })
                        .ToList();

                        if (parameters.Any(string.IsNullOrEmpty) || parameters.Count != route.Parameters.Count)
                            continue;

                        var compiledDelegate = route.Factory.Invoke(parameters.Cast<object>().ToArray());
                        outcome = compiledDelegate.DynamicInvoke(mapping.Host);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}