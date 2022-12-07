using System;
using System.Collections.Generic;

namespace SimAI.Core.Prompts {
    public class PromptWorkflow {
        
        public string Prompt { get; set; }
        public List<PromptWorkflowStep> Steps { get; set; }
        public List<string> Stops { get; set; }

        public PromptWorkflow() {
            Steps = new List<PromptWorkflowStep>();
            Stops = new List<string>();
        }
    }
}