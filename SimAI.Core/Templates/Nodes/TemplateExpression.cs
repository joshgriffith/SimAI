using System.Linq;
using SimAI.Core.Entities;
using SimAI.Core.Parsing;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Templates.Nodes {
    public class TemplateExpression : IsParseNode {
        public string Value { get; set; }
        public string ParameterName { get; set; }
        public EntityDefinition Entity { get; set; }

        public IsParseNode Apply(char token) {
            if (token == '}') {
                if (Value.Contains('=')) {
                    var segments = Value.Split('=');
                    ParameterName = segments.First();
                    Value = segments.Last();
                }

                return null;
            }

            Value += token;
            return this;
        }

        public bool IsMatch(Token token) {
            var value = Value.ToLower();
            return value == token.Type || token.Tags.Any(tag => tag.Key.ToLower() == value);
        }
    }
}