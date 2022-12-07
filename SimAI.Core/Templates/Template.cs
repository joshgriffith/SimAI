using System.Collections.Generic;
using System.Linq;
using SimAI.Core.Extensions;
using SimAI.Core.Parsing;
using SimAI.Core.Templates.Nodes;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Templates {
    public class Template : ParseNodeCollection {
        
        public override IsParseNode Apply(char token) {

            if (token is '}' or ')' or ']' or ' ')
                return this;

            IsParseNode node = token switch {
                '{' => new TemplateExpression(),
                '(' => new TemplateConditional(),
                '[' => new TemplateOptional(),
                _ => null
            };

            if (node != null) {
                Nodes.Add(node);
                return node;
            }

            var literal = new TemplateLiteral();
            literal.Value += token;
            Nodes.Add(literal);
            return literal;
        }

        public override bool IsMatch(Token token) {
            return false;
        }

        public bool IsMatch(TokenSequence sequence) {
            return Find(sequence) >= 0;
        }
        
        public bool IsExactMatch(TokenSequence sequence) {
            return Find(sequence) == 0;
        }

        public int Find(TokenSequence sequence) {
            //var withoutOptional = Nodes.Where(each => each is not TemplateOptional).ToList();
            return sequence.Tokens.Find(Nodes, (token, node) => node.IsMatch(token));
        }

        public IEnumerable<T> GetNodes<T>() where T : IsParseNode {
            var children = Nodes.OfType<T>();
            return children.Concat(Nodes.OfType<Template>().SelectMany(each => each.GetNodes<T>()));
        }
    }
}