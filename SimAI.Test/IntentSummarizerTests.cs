using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Intent;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class IntentSummarizerTests {
        
        [TestMethod]
        public async Task DivisionIntent() {
            var summarizer = new IntentSummarizer(TestHelper.GetProvider());
            var result = await summarizer.GetIntentAsync("8 / 4");
            Assert.AreEqual("divide", result);
        }

        [TestMethod]
        public async Task MathIntent() {
            var summarizer = new IntentSummarizer(TestHelper.GetProvider());
            var result = await summarizer.GetIntentAsync("6 / 3 + 8 * 4");
            Assert.AreEqual("calculate", result);
        }
    }
}