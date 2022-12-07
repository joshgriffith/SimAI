using System;
using System.Collections.Generic;
using System.Linq;
using SimAI.Core.Parsing;
using SimAI.Core.Templates;

namespace SimAI.Core.Intent {
    public class SampleAttribute : Attribute {
        
        public string[] Samples { get; set; }

        public SampleAttribute(params string[] samples) {
            Samples = samples;
        }

        public List<Template> GetTemplates() {
            var parser = new Parser<Template>();
            return Samples.Select(sample => parser.Parse(sample)).ToList();
        }
    }
}