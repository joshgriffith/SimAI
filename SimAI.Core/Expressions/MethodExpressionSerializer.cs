using System;
using System.Text;
using System.Linq.Expressions;

namespace SimAI.Core.Expressions {
    public class MethodExpressionSerializer<T> : ExpressionSerializer<T> {

        protected override string SerializeExpression(Expression expression) {
            var visitor = new DefaultExpressionVisitor();
            visitor.Visit(expression);
            return visitor.ToString()[..^1];
        }

        protected class DefaultExpressionVisitor : ExpressionVisitor {
            private string _result = string.Empty;
            private Expression _previousExpression;

            public override Expression Visit(Expression node) {
                var type = node.GetType().Name;
                _previousExpression = base.Visit(node);
                return _previousExpression;
            }
            
            protected override Expression VisitMethodCall(MethodCallExpression node) {
                var before = _result;
                _result = node.Method.Name + '(';

                var method = base.VisitMethodCall(node);

                _result += ")." + before;

                return method;
            }

            protected override Expression VisitConstant(ConstantExpression node) {
                if (_previousExpression is ConstantExpression)
                    _result += ", ";

                _result += node.Value;
                return base.VisitConstant(node);
            }

            public override string ToString() {
                return _result;
            }
        }
    }
}