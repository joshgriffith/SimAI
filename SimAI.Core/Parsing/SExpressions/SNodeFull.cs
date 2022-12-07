using System;
using System.Collections.Generic;
using System.Text;

namespace SimAI.Core.Parsing.SExpressions {
    public class SNodeFull : SNode {
        private bool _isLeaf;

        public bool IsLeaf {
            get => _isLeaf;
        }

        public SNodeFull(bool isLeaf) : base() {
            this._isLeaf = isLeaf;
        }

        public SNodeFull(string name, bool isLeaf) : base(name) {
            this._isLeaf = isLeaf;
        }

        public SNodeFull RootNode { get; set; }

        public void AddNode(SNodeFull node) {
            base.AddNode(node);
            node.RootNode = this;
        }
    }
}