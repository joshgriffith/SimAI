using SimAI.Core.Parsing;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Templates.Nodes {
    public class TemplateOptional : Template {
        public override IsParseNode Apply(char token) {
            if (token == ']')
                return null;

            return base.Apply(token);
        }

        public override bool IsMatch(Token token) {
            return true;
        }
    }
}