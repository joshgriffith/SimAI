using System;
using System.Collections.Generic;
using System.Linq;
using SimAI.Core.Extensions;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Parsing {
    public abstract class ParseNodeCollection : IsParseNode {
        public List<IsParseNode> Nodes { get; set; } = new();

        public int Size => Nodes.Count;

        public T GetFirst<T>() where T : class, IsParseNode {
            return Nodes.First().As<T>();
        }

        public T GetLast<T>() where T : class, IsParseNode {
            return Nodes.Last().As<T>();
        }

        public abstract IsParseNode Apply(char token);

        public abstract bool IsMatch(Token token);
    }
}