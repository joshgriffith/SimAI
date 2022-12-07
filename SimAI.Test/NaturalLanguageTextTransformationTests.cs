using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Knowledge.CodeGeneration;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class NaturalLanguageTextTransformationTests : BaseTest {
        private const string TestInput = "If John had studied for his test, he wouldn't have received a bad grade.";

        private static async Task<string> Query(string prompt) {
            var query = new StringTransformationQuery(TestHelper.GetProvider());
            var output = await query.Prompt(TestInput, prompt);
            return output;
        }

        [TestMethod]
        public async Task Should_ReplaceWord() {
            var output = await Query("replace the word 'bad' with 'good'");
            Assert.AreEqual("If John had studied for his test, he wouldn't have received a good grade.", output);
        }

        [TestMethod]
        public async Task Should_TakeFirstTwoWords() {
            var output = await Query("the first two words");
            Assert.AreEqual("If John", output);
        }

        [TestMethod]
        public async Task Should_GetWordsStartingWith() {
            var output = await Query("words starting with h");
            Assert.AreEqual("had his he have", output);
        }

        [TestMethod]
        public async Task Should_RemovePunctuation() {
            var output = await Query("exclude punctuation");
            Assert.AreEqual("If John had studied for his test he wouldnt have received a bad grade", output);
        }
    }
}