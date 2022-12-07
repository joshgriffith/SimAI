using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimAI.Core.Entities;
using SimAI.Core.Extensions;
using SimAI.Core.Intent;

namespace SimAI.Core.Skills {
    public class Skill {
        public List<string> Terms { get; set; } = new();
        public List<SkillActuator> Actuators { get; set; } = new();
        public object Reference { get; set; }
        public int Precedence { get; set; }
        public readonly Dictionary<string, EntityDefinition> Entities = new();
        
        public static Skill Map(object reference) {

            if (reference == null)
                throw new NullReferenceException("Skill reference");

            var type = reference.GetType();
            var skill = new Skill();
            skill.Reference = reference;

            type.WithAttribute<SkillAttribute>(attribute => {
                skill.Terms = attribute.Terms.ToList();
            });

            foreach (var attribute in type.GetCustomAttributes().OfType<EntityAttribute>())
                skill.Entities.Add(attribute.Name, new EntityDefinition(attribute.Name, attribute.Patterns.ToArray()));

            if (!skill.Terms.Any())
                skill.Terms.Add(type.Name);

            foreach (var method in type.GetMethods()) {
                if (method.DeclaringType != null && method.DeclaringType.Namespace != null && method.DeclaringType.Namespace.StartsWith("System")) {
                    continue;
                }

                var actuator = new SkillActuator(reference, method);

                method.WithAttribute<IntentPrecedenceAttribute>(attribute => {
                    actuator.Precedence = attribute.Precedence;
                });

                method.WithAttribute<IntentAttribute>(attribute => {
                    actuator.Terms = attribute.Terms.ToList();
                });

                method.WithAttribute<SampleAttribute>(attribute => {
                    actuator.Intents = attribute.GetTemplates();
                });

                //foreach (var expression in actuator.Steps.SelectMany(intent => intent.GetNodes<IntentExpression>()))
                //    expression.Entity = skill.Entities[expression.Value];

                skill.Actuators.Add(actuator);
            }

            skill.Actuators = skill.Actuators.OrderByDescending(actuator => actuator.Precedence).ToList();

            return skill;
        }

        public override string ToString() {
            return GetType().Name;
        }
    }
}