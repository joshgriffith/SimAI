using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SimAI.Core.Extensions;
using SimAI.Core.Json;

namespace SimAI.Core.Prompts {
    public class SerializedPrompt {

        public static SerializedPrompt FromSamples<T>(List<T> samples, T toComplete = default) where T : new() {

            if (toComplete == null)
                toComplete = new T();
            
            var providedProperties = new List<PropertyInfo>();

            foreach (var property in typeof(T).GetProperties()) {
                var value = property.GetValue(toComplete);

                if (value == null || value.Equals(property.PropertyType.Default()))
                    continue;

                providedProperties.Add(property);
            }

            var promptBuilder = new StringBuilder();

            var settings = new JsonSerializerSettings {
                ContractResolver = new PropertyOrderContractResolver(providedProperties),
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            foreach (var sample in samples)
                promptBuilder.AppendLine(JsonConvert.SerializeObject(sample, settings));
            
            var serialized = JsonConvert.SerializeObject(toComplete, settings);
            var fragment = serialized.Substring(0, serialized.Length - 1) + ",";
            promptBuilder.Append(fragment);

            return new SerializedPrompt {
                Body = promptBuilder.ToString(),
                Fragment = fragment
            };
        }

        public string Body { get; set; }
        public string Fragment { get; set; }
    }
}