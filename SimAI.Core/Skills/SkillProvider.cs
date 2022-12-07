using System.Collections.Generic;
using System.Linq;
using SimAI.Core.Intent;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Skills {
    public class SkillProvider {
        public List<Skill> Skills { get; set; } = new();
        /*private EntityProvider _entities;

        public SkillProvider(EntityProvider entities = null) {
            _entities = entities;
        }*/

        public SkillProvider Use(object domain) {
            Skills.Add(Skill.Map(domain));

            //_entities?.Use(domain);

            return this;
        }

        public Skill GetSkillByName(string name) {
            name = name.ToLower();
            return Skills.FirstOrDefault(each => each.Terms.Contains(name));
        }

        public IEnumerable<SkillActuator> GetActuatorsByIntent(string intent) {
            intent = intent.ToLower();
            return Skills
                .SelectMany(skill => skill.Actuators)
                .Where(actuator => actuator.Terms.Contains(intent));
        }

        public IntentBinding GetIntentBinding(TokenSequence query) {
            return Skills
                .SelectMany(skill => skill.Actuators)
                .Select(actuator => actuator.TryGetBinding(query))
                .Where(binding => binding != null)
                .OrderByDescending(binding => binding.Actuator.Precedence)
                .ThenBy(binding => binding.SequenceIndex)
                .FirstOrDefault();
        }
    }
}