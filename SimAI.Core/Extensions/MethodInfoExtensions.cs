using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace SimAI.Core.Extensions {
    public static class MethodInfoExtensions {
        public static T InvokeGenericMethod<T>(this MethodInfo method, object target, Type genericType, params object[] parameters) where T : class {
            return method.MakeGenericMethod(genericType).Invoke(target, parameters).As<T>();
        }

        public static bool IsDynamic(this MethodInfo method) {
            if (method is DynamicMethod || method.Name == "lambda_method") {
                return true;
            }

            return method.GetType() == typeof (MethodInfo);
        }

        public static bool IsExtensionMethod(this MethodInfo method) {
            return method.IsDefined(typeof (ExtensionAttribute), true);
        }

        public static T GetAttribute<T>(this MethodInfo method) where T : Attribute {
            return (T) method.GetCustomAttribute(typeof(T));
        }

        public static void WithAttribute<T>(this MethodInfo method, Action<T> handler) where T : Attribute {
            var attribute = method.GetAttribute<T>();

            if (attribute != null) {
                handler(attribute);
            }
        }
    }
}