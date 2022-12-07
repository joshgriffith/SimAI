using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Skills;
using SimAI.Knowledge.Skills;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class SkillTests {

        [TestMethod]
        public void Should_GetSkillByName() {
            var provider = new SkillProvider();
            provider.Use(new MathSkills());
            var domain = provider.GetSkillByName("math");
            Assert.AreEqual(typeof(MathSkills), domain.Reference.GetType());
        }

        [TestMethod]
        public async Task Should_GetActuatorByQuery() {
            var provider = new SkillProvider();
            provider.Use(new MathSkills());

            var sequence = await TestHelper.Tokenizer.Tokenize("5 plus 5");
            var binding = provider.GetIntentBinding(sequence);

            Assert.IsNotNull(binding);
            Assert.IsTrue(binding.Actuator.Terms.Contains("addition"));
        }

        [TestMethod]
        public void Should_MapSkillActuators() {
            var provider = new SkillProvider();
            provider.Use(new MathSkills());
            var domain = provider.GetSkillByName("math");

            Assert.AreEqual(5, domain.Actuators.Count);

            foreach (var actuator in domain.Actuators) {
                Assert.IsTrue(actuator.Intents.Any());
                Assert.IsTrue(actuator.Terms.Any());

                foreach (var intent in actuator.Intents) {
                    Assert.IsTrue(intent.Nodes.Any());
                }
            }
        }

        [TestMethod]
        public void Should_InvokeKnowledgeActuator() {
            var provider = new SkillProvider();
            provider.Use(new MathSkills());
            var addition = provider.GetActuatorsByIntent("addition").FirstOrDefault();
            var answer = addition.Invoke(3, 3);
            Assert.AreEqual(6, answer);
        }
    }
}