using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Tokenization;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class TokenizerTests : BaseTest {
        private readonly List<IsTokenizer> _tokenizers = new() {
            new CatalystTokenizer(),
            new BasicTokenizer()
        };
        
        [TestMethod]
        public async Task ShouldParse_Word() {
            foreach (var tokenizer in _tokenizers) {
                var input = "test";
                var sequence = await tokenizer.Tokenize(input);

                Assert.AreEqual(1, sequence.Tokens.Count);
                Assert.AreEqual(input, sequence.Tokens.First().Value);
                Assert.AreEqual("word", sequence.Tokens.First().Type);
            }
        }

        [TestMethod]
        public async Task ShouldParse_Number() {
            foreach (var tokenizer in _tokenizers) {
                var input = "123";
                var sequence = await tokenizer.Tokenize(input);

                Assert.AreEqual(1, sequence.Tokens.Count);
                Assert.AreEqual(input, sequence.Tokens.First().Value);
                Assert.AreEqual("number", sequence.Tokens.First().Type);
            }
        }
        
        [TestMethod]
        public async Task ShouldParse_WordAndNumber() {
            foreach (var tokenizer in _tokenizers) {
                var input = "abc 123";
                var sequence = await tokenizer.Tokenize(input);

                Assert.AreEqual(2, sequence.Tokens.Count);
                Assert.AreEqual("abc", sequence.Tokens.First().Value);
                Assert.AreEqual("123", sequence.Tokens.Last().Value);
                Assert.AreEqual("word", sequence.Tokens.First().Type);
                Assert.AreEqual("number", sequence.Tokens.Last().Type);
            }
        }

        [TestMethod]
        public async Task ShouldParse_WordAndSymbol() {
            foreach (var tokenizer in _tokenizers) {

                var input = "test?";
                var sequence = await tokenizer.Tokenize(input);

                Assert.AreEqual(2, sequence.Tokens.Count);
                Assert.AreEqual("test", sequence.Tokens.First().Value);
                Assert.AreEqual("?", sequence.Tokens.Last().Value);
                Assert.AreEqual("word", sequence.Tokens.First().Type);
                Assert.AreEqual("symbol", sequence.Tokens.Last().Type);
            }
        }

        [TestMethod]
        public async Task ShouldParse_DecimalAsNumber() {
            foreach (var tokenizer in _tokenizers) {
                var input = "12.34";
                var sequence = await tokenizer.Tokenize(input);

                Assert.AreEqual(1, sequence.Tokens.Count);
                Assert.AreEqual(input, sequence.Tokens.First().Value);
                Assert.AreEqual("number", sequence.Tokens.First().Type);
            }
        }

        [TestMethod]
        public async Task ShouldParse_NumberAsWord() {
            foreach (var tokenizer in _tokenizers) {
                var input = "abc123";
                var sequence = await tokenizer.Tokenize(input);

                Assert.AreEqual(1, sequence.Tokens.Count);
                Assert.AreEqual(input, sequence.Tokens.First().Value);
                Assert.AreEqual("word", sequence.Tokens.First().Type);
            }
        }

        [TestMethod]
        public async Task ShouldEqual_SingleToken() {
            await With<IsTokenizer>(async tokenizer => {
                var sequence = await tokenizer.Tokenize("test");
                Assert.AreEqual("test", sequence.ToString());
            });
        }

        [TestMethod]
        public async Task ShouldEqual_TokensWithSpaces() {
            await With<IsTokenizer>(async tokenizer => {
                var sequence = await tokenizer.Tokenize("foo bar");
                Assert.AreEqual("foo bar", sequence.ToString());
            });
        }

        [TestMethod]
        public async Task ShouldEqual_TokensWithoutSpaces() {
            await With<IsTokenizer>(async tokenizer => {
                var sequence = await tokenizer.Tokenize("foo:bar");
                Assert.AreEqual("foo:bar", sequence.ToString());
            });
        }

        [TestMethod]
        public async Task Should_Trim() {
            await With<IsTokenizer>(async tokenizer => {
                var sequence = await tokenizer.Tokenize("  foo  bar  ");
                Assert.AreEqual("foo bar", sequence.ToString());
            });
        }

        /*[TestMethod]
        public async Task Should_Lemmatize() {
            await With<IsTokenizer>(async tokenizer => {
                var sequence = await tokenizer.Tokenize("I'm going to begin studying the books.");
                var lemmatized = sequence.ToLemmatized();
            });
        }*/
    }
}