namespace SimAI.Core.Extensions {
    public static class GenericExtensions {
        public static T CloneAs<T>(this object instance) where T : new() {
            var clone = new T();
            var type = instance.GetType();

            foreach (var field in typeof (T).GetProperties()) {
                field.SetValue(clone, type.GetProperty(field.Name).GetValue(instance));
            }

            return clone;
        }
    }
}