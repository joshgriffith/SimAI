using System;
using System.Reflection;

namespace SimAI.Core.Extensions {
    public static class PropertyInfoExtensions {

        public static bool HasAttribute<T>(this PropertyInfo property) where T : Attribute {
            return property.GetCustomAttribute<T>() != null;
        }

        public static bool Is<T>(this PropertyInfo property) {
            return property.PropertyType.IsAssignableFrom(typeof (T));
        }
    }
}