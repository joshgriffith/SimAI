using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Prompts;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class WorkflowTests : BaseTest {
        
        [TestMethod]
        public async Task Should_ReplaceWord() {

            /*var samples = new List<BasePrompt> {
                new ("finish up the task", "FinishTask"),
                new ("close the connection", "CloseConnection"),
            };
            
            var provider = TestHelper.GetProvider();
            var prompt = SerializedPrompt.FromSamples(samples, new BasePrompt("${prompt}"));

            var workflow = new PromptWorkflow();
            workflow.Prompt = "eat a sandwich";
            workflow.AddStep(prompt.Body);

            var result = await workflow.Invoke(provider);
            var combined = SerializedPrompt.FromSamples(samples, new BasePrompt(workflow.Prompt)).Body + result;*/
        }
    }
}