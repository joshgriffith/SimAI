using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimAI.Core.Entities;
using SimAI.Core.Intent;
using SimAI.Core.Templates;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Skills {
    public class SkillActuator {
        public List<string> Terms { get; set; } = new();
        public List<Template> Intents { get; set; } = new();
        public int Precedence { get; set; }
        private readonly MethodInfo _method;
        private readonly object _reference;

        public SkillActuator(object reference, MethodInfo method) {
            _reference = reference;
            _method = method;
        }

        public IntentBinding TryGetBinding(TokenSequence sequence) {

            var match = Intents.Select(template => new {
                Template = template,
                Index = template.Find(sequence)
            })
            .FirstOrDefault(each => each.Index >= 0);
            
            if (match != null) {
                var parameters = _method.GetParameters().Select(each => new EntityParameter(each)).ToList();
                var applicableTokens = sequence.Tokens.Skip(match.Index);
                var matcher = new EntityMatcher();
                
                return new IntentBinding {
                    Actuator = this,
                    Template = match.Template,
                    SequenceIndex = match.Index,
                    ResultType = _method.ReturnType,
                    Entities = matcher.Map(parameters, match.Template, new TokenSequence(applicableTokens.ToArray()))
                };
            }

            return null;
        }
        
        public object Invoke(params object[] parameters) {
            return _method.Invoke(_reference, parameters);
        }

        public object Invoke(IntentBinding binding) {
            var parameters = _method.GetParameters();
            return Invoke(parameters.Select(parameter => binding.Entities.First(each => each.Name == parameter.Name).Value).ToArray());
        }
    }
}