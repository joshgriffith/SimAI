using System;

namespace SimAI.Core.Intent.Dialects {
    public interface IsIntentDialect {
        string Serialize(IntentRoute route);
    }
}