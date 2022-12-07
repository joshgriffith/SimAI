using System;
using System.Linq;
using System.Threading.Tasks;
using SimAI.Core.Skills;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Intent {

    // Find an intent match without performing any actual invocation
    public class IntentPlanner {

        private readonly SkillProvider _skills;
        private readonly IsTokenizer _tokenizer;

        public IntentPlanner(IsTokenizer tokenizer, SkillProvider skills) {
            _tokenizer = tokenizer;
            _skills = skills;
        }

        public async Task<IntentExecutionPlan> GetPlanAsync(string prompt, string intent) {
            var skill = _skills.GetSkillByName(intent);

            if (skill != null) {

            }

            var candidates = _skills.GetActuatorsByIntent(intent);

            if (candidates.Any()) {
                var tokens = await _tokenizer.Tokenize(prompt);


            }

            /*while (true) {
                var binding = _skills.GetIntentBinding(sequence);

                if (binding != null) {
                    if (binding.IsPartial(sequence)) {
                        var token = (await _tokenizer.Tokenize(result.ToString())).Tokens.FirstOrDefault();
                        sequence.Replace(binding.SequenceIndex, binding.Template.Size, token);
                        continue;
                    }
                }

                break;
            }*/
            
            return null;
        }
    }
}