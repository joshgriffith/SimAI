using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SimAI.Core.Extensions;
using SimAI.Core.OpenAI;
using SimAI.Core.Utilities;

namespace SimAI.Core.Intent {

    public class IntentMapping {
        public object Host { get; set; }
        public List<IntentRoute> Routes { get; set; } = new();
    }

    public class IntentMapping<T> : IntentMapping {

        public IntentMapping(object host) {
            Host = host;
        }
        
        public IntentMapping<T> Route(string prompt, Expression<Action<T>> expression) {
            return Route<Action<T>>(new OpenAIFunctionResult {
                In = prompt
            }, expression);
        }

        public IntentMapping<T> Route(string prompt, Expression<Func<T, object>> expression) {
            return Route<Func<T, object>>(new OpenAIFunctionResult {
                In = prompt
            }, expression);
        }

        public IntentMapping<T> Route(IsOpenAISample sample, Expression<Action<T>> expression) {
            return Route<Action<T>>(sample, expression);
        }

        public IntentMapping<T> Route(IsOpenAISample sample, Expression<Func<T, object>> expression) {
            return Route<Func<T, object>>(sample, expression);
        }

        private IntentMapping<T> Route<X>(IsOpenAISample sample, Expression<X> expression) where X : MulticastDelegate {
            var member = expression.GetMember();
            var interceptor = new ExpressionInterceptor();
            interceptor.Visit(expression);

            MulticastDelegate InvocationFactory(params object[] parameters) {
                var parameterMappings = interceptor.Constants
                    .Zip(parameters.ToArray())
                    .ToDictionary(each => each.First, each => {
                        var type = each.First.Value.GetType();
                        return Convert.ChangeType(each.Second, type);
                    });

                var newExpression = new ExpressionInjector(parameterMappings).Visit(expression) as Expression<X>;

                return newExpression.Compile();
            }

            Routes.Add(new IntentRoute {
                Sample = sample,
                Parameters = interceptor.Constants.Select(each => each.Value).ToList(),
                Factory = InvocationFactory,
                MethodName = member.Name.CamelCase()
            });

            return this;
        }
    }
}