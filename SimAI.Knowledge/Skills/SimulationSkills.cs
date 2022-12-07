using System.Collections.Generic;
using SimAI.Core.Intent;
using SimAI.Core.Skills;

namespace SimAI.Knowledge.Skills {

    [Skill("world", "simulation")]
    public class SimulationSkills {
        
        public List<string> Entities { get; set; } = new();

        [Sample("{thing}")]
        public void Create(string thing) {
            Entities.Add(thing);
        }

        [Sample("destroy {thing}", "remove {thing}")]
        public void Destroy(string thing) {
            if(Entities.Contains(thing))
                Entities.Remove(thing);
        }
    }
}