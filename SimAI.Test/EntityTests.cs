using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Entities;
using SimAI.Core.Intent;
using SimAI.Core.Templates;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class EntityTests {
        
        [TestMethod]
        public async Task Should_GetScheme() {
            var definition = new EntityDefinition("scheme", "{letter}:");
            var sequence = await TestHelper.Tokenizer.Tokenize(@"C:");
            Assert.IsTrue(definition.HasMatch(sequence));
        }

        [TestMethod]
        public async Task Should_MatchEntities() {
            var matcher = new EntityMatcher();

            var parameters = new List<EntityParameter> {
                new("value1", typeof(int)),
                new("value2", typeof(int))
            };

            var template = new TemplateParser().Parse("{number} + {number}");
            var input = await TestHelper.Tokenizer.Tokenize("2 + 3");
            var entities = matcher.Map(parameters, template, input);

            Assert.AreEqual(2, entities.Count);
            Assert.IsTrue(entities.Any(entity => entity.Name == "value1"));
            Assert.IsTrue(entities.Any(entity => entity.Name == "value2"));
            Assert.IsTrue(entities.Any(entity => entity.Value.ToString() == "2"));
            Assert.IsTrue(entities.Any(entity => entity.Value.ToString() == "3"));
        }

        /*[TestMethod]
        public async Task Should_GetExtension() {
            var definition = new EntityDefinition("scheme", ".{word, length = 3}");
            var sequence = await TestHelper.Tokenizer.Tokenize(@".txt");
            Assert.IsTrue(definition.HasMatch(sequence));
        }*/

        /*[TestMethod]
        public async Task Should_GetPath() {
            var definition = new EntityDefinition("path", @"{letter}:[\[{word}]]");

            var sequence = await TestHelper.Tokenizer.Tokenize(@"C:");
            Assert.IsTrue(definition.HasMatch(sequence));

            sequence = await TestHelper.Tokenizer.Tokenize(@"C:foo");
            Assert.IsFalse(definition.HasMatch(sequence));

            sequence = await TestHelper.Tokenizer.Tokenize(@"C:\foo");
            Assert.IsTrue(definition.HasMatch(sequence));

            sequence = await TestHelper.Tokenizer.Tokenize(@"C:\foo.txt");
            Assert.IsTrue(definition.HasMatch(sequence));
        }*/

        /*[TestMethod]
        public async Task Should_MapEntities() {
            var sequence = await new BasicTokenizer().Tokenize(@"How many .txt files are there in C:\Code?");
            var provider = TestHelper.GetEntityProvider();
            provider.ApplyEntities(sequence);

            var extension = sequence.FindTokenByTag("entity", "extension");
            Assert.IsNotNull(extension);
            Assert.AreEqual(".txt", extension.Value);
        }*/
    }
}