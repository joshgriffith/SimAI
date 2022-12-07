using System;

namespace SimAI.Core.OpenAI {
    public interface IsOpenAISample {
        string GetOutputValue();
        void SetOutputValue(string value);
    }
}