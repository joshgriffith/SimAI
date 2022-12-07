using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Intent.Dialects;

namespace SimAI.Test {

    [TestClass]
    public class IntentDialectTests {

        [TestMethod]
        public void ShouldSerialize_SExpressionIntentDialect() {
            var dialect = new SExpressionIntentDialect();
            dialect.Deserialize("add 1 1");
        }
    }
}