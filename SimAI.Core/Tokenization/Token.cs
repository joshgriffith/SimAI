using System.Collections.Generic;
using System.Linq;

namespace SimAI.Core.Tokenization {
    public class Token {

        public string Value { get; set; }
        public string Type { get; set; }
        public List<Tag> Tags { get; set; }
        public bool HasSpace { get; set; }
        public string Lemma { get; set; }

        public Token() : this(string.Empty) {
        }

        public Token(string value) {
            Value = value;
            Tags = new List<Tag>();
        }

        public Token(string value, string type) : this(value) {
            Type = type;
        }

        public string GetTagValue(string key) {
            key = key.ToLower();
            return Tags.Where(each => each.Key.ToLower() == key).Select(each => each.Value).FirstOrDefault() ?? string.Empty;
        }

        public bool IsEntityBegin(string key) {
            return GetTagValue(key).ToLower() == "begin";
        }

        public bool IsEntityEnd(string key) {
            return GetTagValue(key).ToLower() == "end";
        }

        public Token Tag(string key) {
            Tags.Add(new Tag(key));
            return this;
        }

        public Token Tag(string key, string value) {
            Tags.Add(new Tag(key, value));
            return this;
        }

        public bool HasTag(string key) {
            key = key.ToLower();
            return Tags.Any(tag => tag.Key == key);
        }
        
        public override string ToString() {
            return Value;
            /*var tags = string.Empty;

            if (Tags.Any())
                tags = "{" + string.Join(',', Tags.Select(tag => tag.ToString())) + "}";

            return (Value + " (" + Type + ") " + tags).Trim();*/
        }
    }
}