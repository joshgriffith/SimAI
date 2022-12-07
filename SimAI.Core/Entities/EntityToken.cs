namespace SimAI.Core.Entities {
    public class EntityToken {
        public string Name { get; set; }
        public object Value { get; set; }

        public EntityToken(string name) {
            Name = name;
        }
    }
}