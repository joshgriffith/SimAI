using SimAI.Core.Intent;
using SimAI.Core.Skills;

namespace SimAI.Knowledge.Skills {

    [Skill("compute", "calculate", "code", "javascript")]
    public class ComputeSkills {

        [Intent("run code", "evaluate code", "compute")]
        [Sample("() => {{*}}")]
        public object Run(string code) {
            var engine = new Jurassic.ScriptEngine();
            return engine.Evaluate(code);
        }
    }
}