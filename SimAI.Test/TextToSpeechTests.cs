using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.TextToSpeech;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class TextToSpeechTests : BaseTest {

        [TestMethod]
        public async Task Test() {
            var message = "Hello Herman";

            await With<IsTextToSpeechProvider>(async provider => {
                await provider.SpeakAsync(message, "Donald Duck");
            });
        }
    }
}