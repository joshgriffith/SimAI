using System;
using System.Linq.Expressions;

namespace SimAI.Core.Expressions {
    public class ExpressionSerializer<T> {

        public string Serialize(Expression<Action<T>> expression) {
            return SerializeExpression(expression);
        }

        public Expression Deserialize(string value) {
            return null;
        }

        protected virtual string SerializeExpression(Expression expression) {
            return expression.ToString();
        }
    }
}