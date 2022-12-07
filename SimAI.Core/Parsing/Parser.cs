using System;

namespace SimAI.Core.Parsing {
    public class Parser<T> where T : IsParseNode, new() {

        public T Parse(string input) {
            var root = new T();
            InternalParse(root, input);
            return root;
        }

        private int InternalParse(IsParseNode parent, string input) {
            var scope = parent;

            for (var position = 0; position < input.Length; position++) {
                scope = scope.Apply(input[position]);

                if (scope == null)
                    return position;

                if (scope != parent && (position + 1) < input.Length)
                    position += InternalParse(scope, input.Substring(position + 1));

                scope = parent;
            }

            return input.Length;
        }
    }
}