using SimAI.Core.Intent;
using SimAI.Core.Logic;
using SimAI.Core.Skills;

namespace SimAI.Knowledge.Skills {
    
    [Skill("logic", "equality", "rationality")]
    public class LogicSkills {
        private readonly LogicGraph _graph = new();

        [Intent("question", "query", "equality")]
        [Sample("is {word} a {word}?")]
        public bool Is(string thing1, string thing2) {
            return _graph.Is(thing1, thing2);
        }

        [Intent("statement", "assignment", "equality", "relationship")]
        [Sample("{word} is a {word} of {word}")]
        public void Add(string thing1, string relationship, string thing2) {
            _graph.Add(thing1, thing2, relationship);
        }

        [Intent("statement", "assignment", "equality", "relationship")]
        [Sample("{word} is a {word}")]
        public void Add(string thing1, string thing2) {
            Add(thing1, thing2, "is");
        }
    }
}