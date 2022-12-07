using System;
using System.Collections.Generic;
using SimAI.Core.Entities;
using SimAI.Core.Skills;
using SimAI.Core.Templates;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Intent {
    public class IntentBinding {
        public List<EntityToken> Entities { get; set; } = new();
        public Template Template { get; set; }
        public int SequenceIndex { get; set; }
        public SkillActuator Actuator { get; set; }
        public Type ResultType { get; set; }

        public bool IsPartial(TokenSequence sequence) {
            return SequenceIndex > 0 || Template.Size < sequence.Size;
        }

        public object Invoke() {
            return Actuator.Invoke(this);
        }
    }
}