using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimAI.Core.Utilities {
    public class ExpressionInterceptor : ExpressionVisitor {

        public List<ConstantExpression> Constants { get; set; } = new();

        protected override Expression VisitConstant(ConstantExpression node) {
            Constants.Add(node);
            return base.VisitConstant(node);
        }
    }
}
