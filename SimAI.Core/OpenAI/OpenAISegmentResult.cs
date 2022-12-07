using System.Collections.Generic;

namespace SimAI.Core.OpenAI {
    public class OpenAISegmentResult {
        public string Prompt { get; set; }
        public List<string> Segments { get; set; }
    }
}