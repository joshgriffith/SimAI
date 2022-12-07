using System.Collections;
using System.Linq;
using SimAI.Core.Intent;
using SimAI.Core.Skills;

namespace SimAI.Knowledge.Skills {
    
    [Skill("math", "arithmetic", "mathematics", "calculate")]
    public class MathSkills {
        
        [IntentPrecedence]
        [Intent("multiply", "multiplication")]
        [Sample("{number} (*|times) {number}", "{number} (multiply|multiplied) by {number}")]
        public int Multiply(int value1, int value2) {
            return value1 * value2;
        }

        [IntentPrecedence]
        [Intent("divide", "division")]
        [Sample("{number} (/|over|divide by|divided by) {number}")]
        public int Divide(int value1, int value2) {
            return value1 / value2;
        }

        [Intent("add", "addition", "sum")]
        [Sample("{number} (+|plus) {number}", "(add|sum) {number} (and|plus|+) {number}", "sum of {number} and {number}")]
        public int Add(int value1, int value2) {
            return value1 + value2;
        }

        [Intent("subtract", "subtraction")]
        [Sample("{number} (-|minus|subtract) {number}", "(subtract|take) {value2=number} from {value1=number}")]
        public int Subtract(int value1, int value2) {
            return value1 - value2;
        }

        [Intent("count", "total")]
        [Sample("how many {*}")]
        public int Count(IEnumerable items) {
            return items.Cast<object>().Count();
        }
    }
}