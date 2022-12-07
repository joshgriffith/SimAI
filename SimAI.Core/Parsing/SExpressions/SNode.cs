using System;
using System.Collections.Generic;
using System.Text;

namespace SimAI.Core.Parsing.SExpressions {
    public class SNode {
        private List<SNode> _items;
        public string Name { get; set; }

        public IReadOnlyCollection<SNode> Items => _items.AsReadOnly();

        public SNode() {
            _items = new List<SNode>();
        }

        public SNode(string name) : this() {
            Name = name;
        }

        public void AddNode(SNode node) {
            _items.Add(node);
        }
    }
}