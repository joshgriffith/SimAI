using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimAI.Core.Entities;

namespace SimAI.Core.Tokenization {
    public class TokenSequence {
        public static readonly TokenSequence Empty = new();

        //public string Value { get; set; }
        public List<Token> Tokens { get; set; } = new();
        public int Size => Tokens.Count;

        public List<EntityReference> Entities { get; set; } = new();

        public TokenSequence() {
            //Value = value;
        }

        public TokenSequence(params Token[] tokens) {
            Tokens = tokens.ToList();
            //Value = string.Join(' ', Tokens.Select(each => each.Value));
        }

        // Should probably be moved to a segmentation pipeline
        public Range GetNextSegment() {
            var start = 0;

            foreach (var token in Tokens) {
                if (token.Value == "(")
                    start = Tokens.IndexOf(token);
                else if (token.Value == ")")
                    return new Range(start, Tokens.IndexOf(token));
            }

            return new Range(0, Size);
        }

        public void AddEntity(string type, Token token) {
            switch (token.GetTagValue(type).ToLower()) {
                case "single":
                    Entities.Add(new EntityReference(type, token));
                    break;
                case "begin":
                    Entities.Add(new EntityReference(type, token));
                    break;
                case "end":
                    Entities.First(each => each.Type == type && !each.Tokens.Any(t => t.IsEntityEnd(type))).Tokens.Add(token);
                    break;
                case "inside":
                    Entities.First(each => each.Type == type && each.Tokens.Any(t => t.IsEntityBegin(type))).Tokens.Add(token);
                    break;
            }
        }
        
        public Token FindTokenByTag(string key, string value) {
            return Tokens.FirstOrDefault(each => each.Tags.Any(tag => tag.Key == key && tag.Value == value));
        }

        public void Replace(int startIndex, int count, Token value) {
            Tokens.RemoveRange(startIndex, count);
            Tokens.Insert(startIndex, value);
        }

        private string ConvertToString(Func<Token, string> valueAccessor) {
            var builder = new StringBuilder();

            foreach (var token in Tokens) {
                if (token.HasSpace)
                    builder.Append(' ');

                builder.Append(valueAccessor(token));
            }

            return builder.ToString();
        }

        public string ToLemmatized() {
            return ConvertToString(token => token.Lemma);
        }

        public override string ToString() {
            return ConvertToString(token => token.Value);
        }
    }
}