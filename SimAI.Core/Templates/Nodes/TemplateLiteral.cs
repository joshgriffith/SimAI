using SimAI.Core.Parsing;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Templates.Nodes {
    public class TemplateLiteral : IsParseNode {
        public string Value { get; set; }

        public IsParseNode Apply(char token) {

            if (token is '{' or '(' or '[' or ' ' or '}' or ')' or ']') {
                Value = Value.Trim();
                return null;
            }

            Value += token;
            return this;
        }

        public bool IsMatch(Token token) {
            return token.Value.ToLower() == Value;
        }

        public override string ToString() {
            return Value;
        }
    }
}