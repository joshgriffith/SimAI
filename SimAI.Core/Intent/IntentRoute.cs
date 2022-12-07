using System;
using System.Collections.Generic;
using SimAI.Core.OpenAI;

namespace SimAI.Core.Intent {
    public class IntentRoute {
        public IsOpenAISample Sample { get; set; }
        public List<object> Parameters { get; set; } = new();
        public string MethodName { get; set; }
        public Func<object[], MulticastDelegate> Factory { get; set; }
    }
}