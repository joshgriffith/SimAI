using System;

namespace SimAI.Core.Skills {
    public class SkillAttribute : Attribute {
        public string[] Terms { get; set; }

        public SkillAttribute(params string[] terms) {
            Terms = terms;
        }
    }
}