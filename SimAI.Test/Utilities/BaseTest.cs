using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Runtime;

namespace SimAI.Test.Utilities {

    [TestClass]
    public abstract class BaseTest {

        private Agent _agent;

        [TestInitialize]
        public void Initialize() {
            _agent = TestHelper.GetAgent();
        }

        [TestCleanup]
        public void Cleanup() {
            _agent.Dispose();
        }

        public async Task With<T>(Func<T, Task> action) {
            await _agent.With(action);
        }
    }
}