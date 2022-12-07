using System.Collections.Generic;
using System.Linq;

namespace SimAI.Core.Logic {
    public class LogicNode {
        public string Term { get; set; }
        public Dictionary<string, List<LogicNode>> Edges = new();

        public LogicNode(string term) {
            Term = term;
        }

        public bool IsTrue(string relationship, string term) {
            if (Edges.ContainsKey(relationship))
                return Edges[relationship].Any(edge => edge.Term == term || edge.IsTrue(relationship, term));

            return false;
        }

        public bool IsFalse(string relationship, string term) {
            return !IsTrue(relationship, term);
        }

        public void Is(string relationship, LogicNode node) {
            if (!Edges.ContainsKey(relationship))
                Edges.Add(relationship, new List<LogicNode>());

            if (!Edges[relationship].Contains(node)) {
                Edges[relationship].Add(node);
            }
        }
    }
}