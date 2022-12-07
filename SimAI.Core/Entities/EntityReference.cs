using System;
using System.Collections.Generic;
using System.Linq;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Entities {
    public class EntityReference {
        public string Type { get; set; }
        public List<Token> Tokens { get; set; }

        public EntityReference(string type, params Token[] tokens) {
            Type = type;
            Tokens = tokens.ToList();
        }

        public string GetValue() {
            return string.Concat(Tokens.Select(each => each.Value));
        }
    }
}