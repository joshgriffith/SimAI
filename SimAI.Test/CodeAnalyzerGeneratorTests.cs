using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Knowledge.CodeGeneration;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class CodeAnalyzerGeneratorTests : BaseTest {

        [TestMethod]
        public async Task Should_GenerateAnalyzer() {
            var generator = new CodeAnalyzerGenerator(TestHelper.GetProvider());
            var code = await generator.Prompt("Warn whenever a local variable is unused");
        }
    }
}