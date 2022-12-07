using System;

namespace SimAI.Core.Intent {
    public class ExampleAttribute : Attribute {
        public string Prompt { get; set; }

        public ExampleAttribute(string prompt) {
            Prompt = prompt;
        }
    }
}
