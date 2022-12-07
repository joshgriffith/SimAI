using System.Collections.Generic;
using System.Linq;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Entities {
    public class EntityProvider {
        private readonly Dictionary<string, EntityDefinition> _mappings = new();

        public List<EntityDefinition> GetPatterns() {
            return _mappings.Values.ToList();
        }

        public EntityProvider Use(object entity) {
            var mapping = EntityDefinition.Map(entity);
            return Use(mapping);
        }

        public EntityProvider Use(EntityDefinition mapping) {
            _mappings.Add(mapping.Name, mapping);
            return this;
        }

        public EntityProvider Use(string name, params string[] samples) {
            return Use(new EntityDefinition(name, samples));
        }

        public void ApplyEntities(TokenSequence sequence) {
            foreach (var token in sequence.Tokens)
                foreach (var match in Find(token.Value))
                    token.Tag("entity", match.Name);
        }

        public IEnumerable<EntityDefinition> Find(string query) {
            return _mappings.Values.Where(each => each.IsMatch(query));
        }
    }
}