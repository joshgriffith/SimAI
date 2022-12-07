using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Templates;
using SimAI.Core.Tokenization;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    // Todo:  Consider merging ParsingTests?

    [TestClass]
    public class TemplateTests : BaseTest {

        // Todo:  Intent mining process (discover / converge on an intent pattern or prompt)
        // Ideally, we want to acquire high-level intent with a single prompt.
        // 'Intent' is the wrong path!  Instead, focus on representing the statement as a solvable expression

        [TestMethod]
        [DataRow(@"\d?", "1")]
        [DataRow("test", "test")]
        [DataRow("(foo )?bar", "foo bar")]
        [DataRow("(foo )?bar", "bar")]
        public async Task Test(string pattern, string input) {
            //(foo)? 
            //pattern = pattern.Replace("{number}", @"\d+");
            //Regex.Replace(pattern, "", "( )?");
            var r = new Regex("");

            Assert.IsTrue(Regex.IsMatch(input, pattern));
        }

        [DataTestMethod]
        [DataRow("{number}", "1")]
        [DataRow("test", "test")]
        //[DataRow("[foo] bar", "foo bar")]
        //[DataRow("[foo] bar", "bar")]
        public async Task ShouldFind_ExactMatch(string pattern, string input) {
            await With<IsTokenizer>(async tokenizer => {
                var template = new TemplateParser().Parse(pattern);
                var sequence = await tokenizer.Tokenize(input);
                Assert.IsTrue(template.IsExactMatch(sequence));
            });
        }

        [DataTestMethod]
        [DataRow("{number}", "{number}")]
        [DataRow("test", "tester")]
        //[DataRow("[foo] bar", "bar bar")]
        public async Task ShouldFind_NoMatch(string pattern, string input) {
            await With<IsTokenizer>(async tokenizer => {
                var template = new TemplateParser().Parse(pattern);
                var sequence = await tokenizer.Tokenize(input);
                Assert.IsFalse(template.IsMatch(sequence));
            });
        }

        [TestMethod]
        public async Task ShouldFind_PartialMatch() {
            await With<IsTokenizer>(async tokenizer => {
                var cache = new TemplateProvider();
                cache.Add("{number}");
                var sequence = await tokenizer.Tokenize("a 2");
                //Assert.IsTrue(cache.HasMatch(sequence));
                Assert.IsFalse(cache.HasExactMatch(sequence));
            });
        }
    }
}
