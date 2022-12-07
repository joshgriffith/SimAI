using System;
using SimAI.Core.Parsing.SExpressions;

namespace SimAI.Core.Intent.Dialects {
    public class SExpressionIntentDialect : IsIntentDialect {
        public string Serialize(IntentRoute route) {
            return null;
        }

        public IntentExecutionPlan Deserialize(string value) {
            var plan = new IntentExecutionPlan();
            var expression = new SExpression();

            if (!value.StartsWith("("))
                value = "(" + value;

            if (!value.EndsWith(")"))
                value += ")";

            var result = expression.Deserialize(value);


            return plan;
        }
    }
}