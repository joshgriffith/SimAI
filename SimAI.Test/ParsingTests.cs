using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Templates;
using SimAI.Core.Templates.Nodes;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    // Resource:  https://docs.microsoft.com/en-us/azure/cognitive-services/luis/reference-pattern-syntax
    
    [TestClass]
    public class ParsingTests {

        [TestMethod]
        public void ShouldParse_Literal() {
            var pattern = "test";
            var template = new TemplateParser().Parse(pattern);

            Assert.AreEqual(1, template.Nodes.Count);
            Assert.AreEqual(pattern, template.GetFirst<TemplateLiteral>().Value);
        }

        [TestMethod]
        public void ShouldParse_Expression() {
            var pattern = "{test}";
            var template = new TemplateParser().Parse(pattern);

            Assert.AreEqual(1, template.Nodes.Count);
            Assert.AreEqual("test", template.GetFirst<TemplateExpression>().Value);
        }

        [TestMethod]
        public async Task ShouldParse_Optional() {
            var pattern = "[hello] world";
            var template = new TemplateParser().Parse(pattern);

            Assert.AreEqual(2, template.Nodes.Count);
            Assert.AreEqual("hello", template.GetFirst<TemplateOptional>().GetFirst<TemplateLiteral>().Value);
            Assert.AreEqual("world", template.GetLast<TemplateLiteral>().Value);

            var sequence = await TestHelper.Tokenizer.Tokenize("hello world");
            Assert.IsTrue(template.Find(sequence) >= 0);

            sequence = await TestHelper.Tokenizer.Tokenize("world");
            Assert.IsTrue(template.Find(sequence) >= 0);
        }

        /*[TestMethod]
        public void ShouldParse_ConditionalOptional() {
            var pattern = "(foo|[foo]bar)";
            var parser = new Tokenizer<IntentTemplate>();
            var template = parser.Tokenize(pattern);

            Assert.AreEqual(1, template.Nodes.Count);
            Assert.AreEqual("test", template.GetFirst<IntentOptional>().Value);
        }*/

        [TestMethod]
        public void ShouldParse_ExpressionWithParameter() {
            var pattern = "{testparameter=test}";
            var template = new TemplateParser().Parse(pattern);

            Assert.AreEqual(1, template.Nodes.Count);
            Assert.AreEqual("test", template.GetFirst<TemplateExpression>().Value);
            Assert.AreEqual("testparameter", template.GetFirst<TemplateExpression>().ParameterName);
        }

        [TestMethod]
        public void ShouldParse_MultipleLiterals_And_MultipleExpressions() {
            var pattern = "foo bar {expression} hello world";
            var template = new TemplateParser().Parse(pattern);

            Assert.AreEqual(5, template.Nodes.Count);

            Assert.AreEqual("foo", template.GetFirst<TemplateLiteral>().Value);
            Assert.AreEqual("expression", template.Nodes.OfType<TemplateExpression>().First().Value);
            Assert.AreEqual("world", template.GetLast<TemplateLiteral>().Value);
        }

        [TestMethod]
        public void ShouldParse_Conditional() {
            var pattern = "(dog|cat)";
            var template = new TemplateParser().Parse(pattern);

            Assert.AreEqual(1, template.Nodes.Count);

            var expression = template.Nodes.OfType<TemplateConditional>().First();

            Assert.AreEqual("dog", expression.Values.First());
            Assert.AreEqual("cat", expression.Values.Last());
        }
    }
}