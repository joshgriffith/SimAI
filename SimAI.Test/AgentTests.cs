using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.ResponseHandlers;
using SimAI.Core.Runtime;
using SimAI.Test.Middleware;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class AgentTests {
        
        [TestMethod]
        public async Task Should_UseMiddleware() {
            var agent = new Agent();
            agent.Use<TestMiddleware>();
            var response = await agent.Execute("ping");
            Assert.AreEqual("pong", response);
        }

        [TestMethod]
        public async Task ClearData() {
            //await GetProvider().DeleteAll();
        }
        
        [TestMethod]
        public async Task Should_GetInvalidResponse() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute("wtf");
            Assert.AreEqual(UnknownRequestHandler.DefaultMessage, response);
        }

        [TestMethod]
        public async Task Should_GetBasicAnswer() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute("2 + 2");
            Assert.AreEqual("4", response);
        }

        [TestMethod]
        public async Task Should_GetMultistepAnswer() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute("1 + 2 + 3");
            Assert.AreEqual("6", response);
        }

        [TestMethod]
        public async Task Should_GetOrderedMultistepAnswer() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute("10 - 2 * 2");
            Assert.AreEqual("6", response);
        }

        [TestMethod]
        public async Task Should_GetAnswerWithParantheses() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute("24 / ((10 - 2) + 4) * 2");
            Assert.AreEqual("4", response);
        }

        /*[TestMethod]
        public async Task Should_GetAnswerWithComplexIntent() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute(@"How many .txt files are there in C:\Code?");
            Assert.AreEqual("1", response);
        }*/

        [TestMethod]
        public async Task Should_GetAnswerWithEntity() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute(@"what files are at C:\Code");
        }

        [TestMethod]
        public async Task Should_GetAnswerWithCompoundIntent() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute(@"how many files are in C:\Code");
        }

        [TestMethod]
        public async Task Should_GetTime() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute("What time is it?");
            Assert.IsTrue(DateTime.TryParse(response, out _));
        }

        //[TestMethod]
        public async Task TestCreateEntity() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute("an icecube is here");
            throw new Exception();
            //Assert.IsTrue(agent.Entities.Contains("an icecube"));
            Assert.AreEqual(response, "an icecube appears");
        }

        //[TestMethod]
        public async Task TestSetTime() {
            var agent = TestHelper.GetAgent();
            var response = await agent.Execute("An hour passes");
            var timeQuery = await agent.Execute("What time is it?");
            Assert.AreEqual(response, timeQuery);
        }

        //[TestMethod]
        public async Task Causation() {
            var agent = TestHelper.GetAgent();
            await agent.Execute("an icecube is here");
            var response = await agent.Execute("An hour passes");


            /* Todo
             { given: "an icecube", when: "an hour passes", then: "it melts" }
             { given: "an icecube", when: "it melts", then: "it becomes water" }
             { prompt: "an icecube becomes water", intent: "change an icecube to water" }
            */
        }
    }
}