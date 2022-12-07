using System;
using System.Collections.Generic;

namespace SimAI.Core.Intent {
    public class IntentExecutionStep {
        public List<IsIntentParameter> Parameters { get; set; } = new();
        public string Verb { get; set; }
        //public List<EntityToken> Entities { get; set; } = new();
        //public MethodInfo Method { get; set; }
    }
}