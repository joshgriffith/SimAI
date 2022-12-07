using System;

namespace SimAI.Core.Tokenization {
    public interface IsTokenizationStep {
        string Process(string input);
    }
}
