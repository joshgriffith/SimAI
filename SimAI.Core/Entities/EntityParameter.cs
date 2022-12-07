using System;
using System.Reflection;

namespace SimAI.Core.Entities {
    public class EntityParameter {
        public string Name { get; set; }
        public Type Type { get; set; }

        public EntityParameter(ParameterInfo parameter) : this(parameter.Name, parameter.ParameterType) {
        }

        public EntityParameter(string name, Type type) {
            Name = name;
            Type = type;
        }
    }
}
