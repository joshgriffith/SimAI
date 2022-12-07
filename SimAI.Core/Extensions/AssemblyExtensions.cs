using System;
using System.Linq;
using System.Reflection;

namespace SimAI.Core.Extensions {
    public static class AssemblyExtensions {
        public static Type GetTypeByName(this Assembly assembly, string typeName) {
            return assembly.GetTypes().FirstOrDefault(each => each.Name.ToLower() == typeName.ToLower());
        }
    }
}