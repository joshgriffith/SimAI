using System;

namespace SimAI.Core.Intent {
    public class IntentAttribute : Attribute {
        public string[] Terms { get; set; }

        public IntentAttribute(params string[] terms) {
            Terms = terms;
        }
    }
}