using System;
using System.Linq;

namespace SimAI.Core.Intent.Dialects {
    public class ImperativeIntentDialect : IsIntentDialect {

        public IntentExecutionPlan Deserialize(string value) {
            
            var plan = new IntentExecutionPlan();

            foreach (var token in value) {

                /*switch (token) {
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
                }*/


                /*var parameters = new List<string>();
                var parameter = string.Empty;
                var depth = 0;

                foreach(var token in function) {
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
                }*/

                /*if (parameter.Length > 0) {
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
                    continue;*/
            }
            
            return plan;
        }

        private IntentExecutionStep Parse(string value) {
            var step = new IntentExecutionStep();

            foreach (var token in value) {

                switch (token) {
                    case '.':
                        return step;
                    case '(':
                        // 
                        break;
                    case ')':
                        break;
                    case '\'':
                        break;
                    case ',':
                        /*if (depth == 0) {
                            parameters.Add(parameter);
                            parameter = string.Empty;
                        }
                        else
                            parameter += token;*/

                        break;
                    default:
                        /*if (token == '(')
                            depth += 1;
                        else if (token == ')')
                            depth -= 1;

                        parameter += token;*/
                        break;
                }
            }

            return step;
        }

        public string Serialize(IntentRoute route) {
            var parameters = route.Parameters.Select(each => {
                if (each is string)
                    return "'" + each + "'";

                return each;
            });

            return route.MethodName + "(" + string.Join(',', parameters.ToArray()) + ")";
        }
    }
}