using System.Collections.Generic;
using System.Linq;
using SimAI.Core.Parsing;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Templates.Nodes {
    public class TemplateConditional : IsParseNode {
        public List<string> Values { get; set; } = new();

        public IsParseNode Apply(char token) {

            switch (token) {
                case ')':
                    return null;
                case '|':
                    Values.Add(string.Empty);
                    break;
                default:
                    if(!Values.Any())
                        Values.Add(string.Empty);

                    Values[^1] += token;
                    break;
            }

            return this;
        }

        public bool IsMatch(Token token) {
            return Values.Contains(token.Value.ToLower());
        }

        public class ConditionalToken {
            public string Value { get; set; }
            public bool IsOptional { get; set; }

            public ConditionalToken() {
                Value = string.Empty;
            }
        }
    }
}