using SimAI.Core.Extensions;

namespace SimAI.Core.Tokenization {
    public struct Tag {

        public Tag(string key) : this(key, string.Empty) {
        }

        public Tag(string key, string value) {
            Key = key.ToLower();
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public override string ToString() {
            return Key + (Value.IsEmpty() ? "" : "=" + Value);
        }
    }
}
