using System;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Parsing {
    public interface IsParseNode {
        public IsParseNode Apply(char token);
        bool IsMatch(Token token);
    }
}