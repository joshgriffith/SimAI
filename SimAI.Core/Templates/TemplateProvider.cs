using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Templates {
    public class TemplateProvider {
        private TemplateParser _parser = new();
        private string _pattern;
        private readonly List<Template> _templates = new();
        private readonly HashSet<string> _samples = new();
        //private readonly Dictionary<Template, string> _cache = new();

        public void AddMatch(TokenSequence sequence) {
            if (!_samples.Any())
                _pattern = sequence.ToString();
            else {
                var pattern = new StringBuilder();

                // what is 2 + 3
                // 9 + 3
                // [what is] {number} + 3

                foreach (var token in sequence.Tokens) {

                }

                _pattern = pattern.ToString();
            }

            _samples.Add(sequence.ToString());
        }

        public void Add(string template) {
            Add(_parser.Parse(template));
        }

        public void Add(Template template) {
            _templates.Add(template);
        }

        public decimal IsMatch(TokenSequence sequence) {
            return Regex.IsMatch(sequence.ToString(), _pattern) ? 1 : 0;
        }

        public bool HasExactMatch(TokenSequence sequence) {
            return Regex.IsMatch(sequence.ToString(), _pattern);
            //return _samples.Contains(sequence.ToString());
        }

        /*public Template FindMatch(TokenSequence sequence) {
            return _templates.FirstOrDefault(template => template.Find(sequence) >= 0);
        }

        public bool HasMatch(TokenSequence sequence) {
            return FindMatch(sequence) != null;
        }

        public Template FindExactMatch(TokenSequence sequence) {
            return _templates.FirstOrDefault(template => template.Find(sequence) == 0);
        }

        public bool HasExactMatch(TokenSequence sequence) {
            return FindExactMatch(sequence) != null;
        }*/
    }
}