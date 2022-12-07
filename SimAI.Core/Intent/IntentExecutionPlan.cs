using System;
using System.Collections.Generic;

namespace SimAI.Core.Intent {
    public class IntentExecutionPlan {
        public List<IntentExecutionStep> Steps { get; set; } = new();
    }
}