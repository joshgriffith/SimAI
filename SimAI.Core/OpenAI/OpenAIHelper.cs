using SimAI.Core.Extensions;

namespace SimAI.Core.OpenAI {
    public static class OpenAIHelper {
        public static string GetEngineName(OpenAIEngines engine) {
            return engine.ToString().SplitCamelCase('-').ToLower();
        }
    }
}