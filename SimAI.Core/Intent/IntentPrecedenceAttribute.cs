using System;

namespace SimAI.Core.Intent {
    public class IntentPrecedenceAttribute : Attribute {
        public int Precedence { get; set; }

        public IntentPrecedenceAttribute(int precedence = 1) {
            Precedence = precedence;
        }
    }
}