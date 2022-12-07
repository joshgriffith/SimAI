using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Intent.Dialects;
using SimAI.Core.Skills;
using SimAI.Knowledge.Skills;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class IntentTests {
        
        // Todo:  Narrow down domains / intents based on availability of entities

        // Todo:  Domain classifier (Math, Time, Logic, etc)
        // Todo:  Intent classifier (actual method)
        // Todo:  Entity recognizer (parameters)
        // Todo:  Role classifier (which entity matches which parameter)

        private SkillProvider GetSkillProvider() {
            var provider = new SkillProvider();
            provider.Use(new MathSkills());
            return provider;
        }

        /*[TestMethod]
        public async Task ShouldGet_ExecutionPlan() {
            var agent = TestHelper.GetAgent();

            await agent.With<IntentPlanner>(async planner => {
                var plan = await planner.GetPlanAsync("2 + 2", "add");

            });
        }*/
        [TestMethod]
        public void ShouldDeserialize_ImperativeIntentDialect() {
            var dialect = new ImperativeIntentDialect();

        }

        [TestMethod]
        public async Task ShouldInvoke_IntentBinding() {
            var provider = GetSkillProvider();
            var sequence = await TestHelper.Tokenizer.Tokenize("sum of 2 and 2");
            var binding = provider.GetIntentBinding(sequence);
            Assert.IsNotNull(binding);

            var answer = binding.Invoke();
            Assert.AreEqual(4, answer);
        }

        [TestMethod]
        public async Task ShouldInvoke_IntentBinding_WithReversedParameters() {
            var provider = GetSkillProvider();
            var sequence = await TestHelper.Tokenizer.Tokenize("subtract 5 from 8");
            var binding = provider.GetIntentBinding(sequence);
            Assert.IsNotNull(binding);

            var answer = binding.Invoke();
            Assert.AreEqual(3, answer);
        }

        /*[TestMethod]
        public void ShouldInvoke_IntentBinding_WithOptionalParameters() {
            var provider = GetSkillProvider();

            var binding1 = provider.GetIntentBinding("12 divided by 6");
            var binding2 = provider.GetIntentBinding("12 divide 6");
            var binding3 = provider.GetIntentBinding("3 multiply by 3");
            var binding4 = provider.GetIntentBinding("3 times 3");

            Assert.IsNotNull(binding1);
            Assert.IsNotNull(binding2);
            Assert.IsNotNull(binding3);
            Assert.IsNotNull(binding4);
            
            Assert.AreEqual(2, binding1.Invoke());
            Assert.AreEqual(2, binding2.Invoke());

            Assert.AreEqual(9, binding3.Invoke());
            Assert.AreEqual(9, binding4.Invoke());
        }*/
    }
}