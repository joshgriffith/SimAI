using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.OpenAI;
using SimAI.Test.Mocks;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class OpenAIProviderTests : BaseTest {

        private readonly List<OpenAIFunctionResult> _functionSamples = new() {
            new() { In = "multiply 3 and 6", Out = "multiply(3, 6)" },
            new() { In = "restart the computer at 3 pm", Out = "at('3 pm').restart('computer')" },
            new() { In = "a woman is a human", Out = "is('woman', 'human')" },
            new() { In = @"list of files in d:\foo\bar", Out = @"getFiles('d:\foo\bar')" },
            new() { In = "click submit after you sign the document", Out = "sign('document').click('submit')" },
            new() { In = "text me a list of your favorite foods", Out = "sendText('me', list('favorite foods'))" },
            new() { In = "email me a cat", Out = "email('me', findPicture('cat'))" },
            new() { In = "count the shopping list", Out = @"getShoppingList().count()" },
            new() { In = "i have 2 marbles in one hand and 3 marbles in the other hand, how many marbles do i have", Out = "add(2, 3)" }
        };

        private static readonly List<JsonCompletionTestModel> _samples = new() {
            new() {Name = "John Smith", Age = 32, IsMarried = false, State = "Maryland"},
            new() {Name = "Jane Doe", Age = 24, IsMarried = true, State = "North Carolina"},
            new() {Name = "Andy Thompson", Age = 9, IsMarried = false, State = "California"}
        };

        [TestMethod]
        public async Task ShouldComplete_BasicJson() {
            await With<OpenAIProvider>(async provider => {

                var completion = new JsonCompletionTestModel {
                    Name = "Bobby Joe"
                };

                var result = await provider.Completion(_samples, completion);

                Assert.AreEqual(result, completion);
                Assert.AreEqual("Bobby Joe", result.Name);
                Assert.AreEqual(17, result.Age);
                Assert.AreEqual(false, result.IsMarried);
                Assert.AreEqual("Florida", result.State);
            });
        }

        [TestMethod]
        public async Task ShouldComplete_OrderedJson() {
            await With<OpenAIProvider>(async provider => {

                var completion = new JsonCompletionTestModel {
                    Age = 14,
                    State = "Alabama"
                };

                var result = await provider.Completion(_samples, completion);

                Assert.AreEqual(result, completion);
                Assert.AreEqual("Bob Davis", result.Name);
                Assert.AreEqual(14, result.Age);
                Assert.AreEqual(true, result.IsMarried);
                Assert.AreEqual("Alabama", result.State);
            });
        }

        [TestMethod]
        public async Task ShouldCreateFunction_FromWordProblem() {
            await With<OpenAIProvider>(async provider => {
                var result = await provider.ToFunctionAsync("There are six cows, and five run away, how many cows do I have left", _functionSamples.ToArray());
                Assert.AreEqual("subtract(6, 5)", result);
            });
        }

        [TestMethod]
        public async Task ShouldCreateFunction_FromSequential() {
            await With<OpenAIProvider>(async provider => {
                var result = await provider.ToFunctionAsync("call me after you buy lunch", _functionSamples.ToArray());
                Assert.AreEqual("buy('lunch').call('me')", result);
            });
        }

        [TestMethod]
        public async Task ShouldCreateFunction_FromNested() {
            await With<OpenAIProvider>(async provider => {
                var result = await provider.ToFunctionAsync(@"count all of the image files in c:\foo", _functionSamples.ToArray());
                Assert.AreEqual(@"getFiles('c:\foo').count()", result);
            });
        }

        [TestMethod]
        public async Task ShouldCreateFunction_FromFact() {
            await With<OpenAIProvider>(async provider => {
                var result = await provider.ToFunctionAsync("a dog is an animal", _functionSamples.ToArray());
                Assert.AreEqual("is('dog', 'animal')", result);
            });
        }

        /*[TestMethod]
        public async Task ShouldCreateFunction_FromNested() {
            var provider = TestHelper.GetProvider();
            var result = await provider.ToFunctionAsync(@"count the number of files at c:\foo");
            Assert.AreEqual(@"count('c:\foo')", result);
        }*/
    }
}