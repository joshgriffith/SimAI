using System.Collections.Generic;
using SimAI.Core.Extensions;

namespace SimAI.Core.Logic {
    public class LogicGraph {
        private readonly Dictionary<string, LogicNode> _logicGraph = new();
        
        public bool Is(string thing1, string thing2 = "", string relationship = "is") {

            if (_logicGraph.ContainsKey(thing1)) {
                if (!thing2.IsEmpty())
                    return _logicGraph[thing1].IsTrue(relationship, thing2);

                return true;
            }

            return false;
        }
        
        public void Add(string thing1, string thing2, string relationship = "is") {
            if (!_logicGraph.ContainsKey(thing1))
                _logicGraph.Add(thing1, new LogicNode(thing1));

            if (!thing2.IsEmpty()) {
                if (!_logicGraph.ContainsKey(thing2))
                    _logicGraph.Add(thing2, new LogicNode(thing2));

                _logicGraph[thing1].Is(relationship, _logicGraph[thing2]);
            }
        }
        
        public void Add(string thing1, string thing2 = "") {
            Add(thing1, thing2, "is");
        }
    }
}