using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimAI.Core.Utilities {
    public class ExpressionInjector : ExpressionVisitor {
        public Dictionary<ConstantExpression, object> Parameters { get; set; } = new();

        public ExpressionInjector(Dictionary<ConstantExpression, object> parameters) {
            Parameters = parameters;
        }

        protected override Expression VisitConstant(ConstantExpression node) {
            return Expression.Constant(Parameters[node]);
        }
    }
}