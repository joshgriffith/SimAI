using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Expressions;

namespace SimAI.Test {

    [TestClass]
    public class ExpressionTests {

        protected class ExpressionTestObject {
            public void ParameterlessMethod() {}
            public ExpressionTestObject MethodWithParameters(int a, int b) { return this; }
            public ExpressionTestObject ParameterlessMethodWithReturnValue() { return this; }
        }

        private MethodExpressionSerializer<ExpressionTestObject> GetSerializer() {
            return new();
        }

        [TestMethod]
        public void ShouldSerialize_ParameterlessMethod() {
            var result = GetSerializer().Serialize(x => x.ParameterlessMethod());
            Assert.AreEqual("ParameterlessMethod()", result);
        }

        [TestMethod]
        public void ShouldSerialize_MethodChain() {
            var result = GetSerializer().Serialize(x => x.ParameterlessMethodWithReturnValue().ParameterlessMethod());
            Assert.AreEqual("ParameterlessMethodWithReturnValue().ParameterlessMethod()", result);
        }

        [TestMethod]
        public void ShouldSerialize_MethodParameterConstants() {
            var result = GetSerializer().Serialize(x => x.MethodWithParameters(1, 3));
            Assert.AreEqual("MethodWithParameters(1, 3)", result);
        }

        [TestMethod]
        public void ShouldSerialize_ChainedMethodsWithParameters() {
            var result = GetSerializer().Serialize(x => x.MethodWithParameters(1, 2).MethodWithParameters(3, 4));
            Assert.AreEqual("MethodWithParameters(1, 2).MethodWithParameters(3, 4)", result);
        }
    }
}