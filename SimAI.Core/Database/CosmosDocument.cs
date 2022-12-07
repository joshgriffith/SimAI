namespace SimAI.Core.Database {
    public abstract class CosmosDocument : IsPartitioned {
        public string Id { get; set; }
        public string Partition { get; set; }
    }
}