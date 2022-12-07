namespace SimAI.Core.Tokenization.Steps {
    public class StringTrimStep : IsTokenizationStep {
        public string Process(string input) {
            return input.Trim();
        }
    }
}
