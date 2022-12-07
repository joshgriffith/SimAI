using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Knowledge.CodeGeneration;
using SimAI.Test.Mocks;
using SimAI.Test.Utilities;

namespace SimAI.Test {
    
    [TestClass]
    public class NaturalLanguageQueryTests : BaseTest {

        private async Task<string> Query<T>(string prompt) {
            var query = new NaturalLanguageQuery(TestHelper.GetProvider());
            return await query.Prompt<T>(prompt);
        }

        [TestMethod]
        public async Task ShouldQuery_Where() {
            var result = await Query<Person>("legal adults");
            Assert.AreEqual("Where(x => x.Age >= 18);", result);
        }

        [TestMethod]
        public async Task ShouldQuery_WhereAnd() {
            var result = await Query<Person>("people named Bob that live in 27604");
            Assert.AreEqual("Where(x => x.FirstName == \"Bob\" && x.Address.ZipCode == 27604);", result);
        }

        [TestMethod]
        public async Task ShouldQuery_WhereCount() {
            var result = await Query<Person>("count how many people live in Raleigh");
            Assert.AreEqual("Where(x => x.Address.City == \"Raleigh\").Count();", result);
        }

        [TestMethod]
        public async Task ShouldQuery_GroupBy() {
            var result = await Query<Person>("group everyone by the first letter of their last name");
            Assert.AreEqual("GroupBy(x => x.LastName.Substring(0, 1));", result);
        }
    }
}