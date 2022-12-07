using System;
using System.Collections.Generic;
using System.Linq;
using SimAI.Core.Extensions;
using SimAI.Core.Templates;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Entities {
    public class EntityDefinition {
        public string Name { get; set; }
        public List<Template> Templates { get; set; }

        public EntityDefinition(string name, params string[] samples) {
            Name = name;

            var parser = new TemplateParser();
            Templates = samples.Select(sample => parser.Parse(sample)).ToList();
        }

        public bool HasMatch(TokenSequence sequence) {
            return Templates.Any(template => template.Find(sequence) >= 0);
        }

        public bool IsMatch(string pattern) {
            return false;
            //return Predicate(pattern);
        }

        public static EntityDefinition Map(object reference) {
            if (reference == null)
                throw new NullReferenceException("Entity reference");

            var type = reference.GetType();
            var attribute = type.GetAttribute<EntityAttribute>();

            if (attribute == null)
                throw new Exception("Tried to map entity without EntityAttribute: " + reference.GetType().Name);
            
            return new EntityDefinition(attribute.Name, attribute.Patterns.ToArray());
        }
    }
}