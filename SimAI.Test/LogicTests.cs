using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Logic;

namespace SimAI.Test {

    [TestClass]
    public class LogicTests {

        [TestMethod]
        public void TestExists() {
            var graph = new LogicGraph();
            Assert.IsFalse(graph.Is("dog"));

            graph.Add("dog");
            Assert.IsTrue(graph.Is("dog"));
        }

        [TestMethod]
        public void TestEquality() {
            var graph = new LogicGraph();
            Assert.IsFalse(graph.Is("dog"));

            graph.Add("dog", "animal");
            Assert.IsTrue(graph.Is("dog", "animal"));
            Assert.IsFalse(graph.Is("animal", "dog"));
        }

        [TestMethod]
        public void TestImplication() {
            var graph = new LogicGraph();
            graph.Add("fluffy", "soft");
            graph.Add("bunny", "fluffy");
            Assert.IsTrue(graph.Is("bunny", "soft"));
            Assert.IsTrue(graph.Is("bunny", "fluffy"));
        }
    }
}