using System;
using System.Collections.Generic;
using System.Linq;

namespace SimAI.Core.Entities {
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class EntityAttribute : Attribute {
        public string Name { get; set; }
        public List<string> Patterns { get; set; }
        
        public EntityAttribute(string name, params string[] patterns) {
            Name = name;
            Patterns = patterns.ToList();
        }
    }
}