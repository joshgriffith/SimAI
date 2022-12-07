using System;

namespace SimAI.Core.Prompts {
    public class PromptWorkflowStep {
        public string Prompt { get; set; }

        public PromptWorkflowStep() {
        }

        public PromptWorkflowStep(string prompt) {
            Prompt = prompt;
        }
    }
}