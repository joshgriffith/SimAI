using System;

namespace SimAI.Core.Prompts {
    public class BasePrompt {
        public string In { get; set; }
        public string Out { get; set; }

        public BasePrompt() {
        }

        public BasePrompt(string inValue) {
            In = inValue;
        }

        public BasePrompt(string inValue, string outValue) {
            In = inValue;
            Out = outValue;
        }

        public string GetOutputValue() {
            return Out;
        }

        public void SetOutputValue(string value) {
            Out = value;
        }
    }
}