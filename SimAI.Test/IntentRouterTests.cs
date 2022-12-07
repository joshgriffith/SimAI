using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimAI.Core.Intent;
using SimAI.Knowledge.Domain;
using SimAI.Knowledge.Skills;
using SimAI.Test.Mocks;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class IntentRouterTests : BaseTest {

        [TestMethod]
        public async Task ShouldMapIntent_WithReturnValue() {
            await With<IntentRouter>(async router => {
                router.Use(new Clock()).Route("what time is it?", clock => clock.GetTime());

                Assert.AreEqual(1, router.Mappings.Count);
                Assert.AreEqual("getTime()", router.Mappings.First().Routes.First().ToString());
            });
        }

        [TestMethod]
        public async Task ShouldMapIntent_WithParameter() {
            await With<IntentRouter>(async router => {
                router.Use(new Clock())
                    .Route("set the clock to 10:57 pm", clock => clock.SetTime(DateTime.Parse("10:57 pm")));

                Assert.AreEqual(1, router.Mappings.Count);
                Assert.AreEqual("setTime('10:57 pm')", router.Mappings.First().Routes.First().ToString());
            });
        }

        [TestMethod]
        public async Task ShouldInvokeRoute_WithReturnValue() {
            await With<IntentRouter>(async router => {
                var time = DateTime.Parse("1/1/2021 1:00 PM");

                router.Use(new Clock(time))
                    .Route("what time is it?", clock => clock.GetTime())
                    .Route("set the clock to 10:57 pm", clock => clock.SetTime(DateTime.Parse("10:57 pm"))
                );

                var result = await router.Prompt("what's the currrent time");

                Assert.AreEqual(time, result);
            });
        }

        [TestMethod]
        public async Task ShouldInvokeRoute_WithDualIntent() {
            await With<IntentRouter>(async router => {
                var time = DateTime.Parse("1/1/2021 1:00 PM");

                router.Use(new Clock(time))
                    .Route("what time is it?", clock => clock.GetTime())
                    .Route("set the clock to 10:57 pm", clock => clock.SetTime(DateTime.Parse("10:57 pm")))
                    .Route("add an hour", clock => clock.AddTime(new TimeSpan(1, 0, 0))
                );

                router.Use(new MathSkills())
                    .Route("8 + 3", math => math.Add(8, 3))
                    .Route("what is 9 * 2?", math => math.Multiply(9, 2));

                await router.Prompt("multiply 3*3 as minutes to the current time");
                var result = await router.Prompt("what's the currrent time");

                Assert.IsTrue(result is DateTime);
                Assert.AreEqual("1/1/2021 1:09:00 PM", result.ToString());
            });
        }

        [TestMethod]
        public async Task ShouldInvokeRoute_WithParameters() {
            var time = DateTime.Parse("1/1/2021 1:00 PM");

            await With<IntentRouter>(async router => {

                router.Use(new Clock(time))
                    .Route("what time is it?", clock => clock.GetTime())
                    .Route("set the clock to 3/5/1997 10:57 pm", clock => clock.SetTime(DateTime.Parse("3/5/1997 10:57 pm"))
                );

                await router.Prompt("set time as 1/1/2019 3:00 am");
                var result = await router.Prompt("now what time?");

                Assert.IsTrue(result is DateTime);
                Assert.AreEqual("1/1/2019 3:00:00 AM", result.ToString());
            });
        }

        [TestMethod]
        public async Task ShouldInvokeRoute_WithNumericParameters() {
            await With<IntentRouter>(async router => {
                var time = DateTime.Parse("1/1/2021 1:00 PM");

                router.Use(new Clock(time))
                    .Route("what time is it?", clock => clock.GetTime())
                    .Route("add seventeen seconds", clock => clock.SetTime(clock.GetTime().Add(new TimeSpan(0, 0, 0, 17))))
                    .Route("add an hour", clock => clock.SetTime(clock.GetTime().Add(new TimeSpan(0, 1, 0, 0)))
                );

                await router.Prompt("add a couple minutes");
                var result = await router.Prompt("now what time?");

                Assert.IsTrue(result is DateTime);
                Assert.AreEqual("1/1/2021 1:02:00 PM", result.ToString());
            });
        }

        /*[TestMethod]
        public async Task ShouldInvokeRoute_WithMultipleIntents() {
            var time = DateTime.Parse("1/1/2021 1:00 PM");

            await With<IntentRouter>(async router => {

                router.Use(new Clock(time))
                    .Route("what time is it?", clock => clock.GetTime())
                    .Route("add an hour", clock => clock.AddTime(new TimeSpan(1, 0, 0))
                );

                router.Use(new Person("Josh", 38))
                    .Route("How old is Josh?", person => person.GetAge());

                await router.Prompt("add Josh's age as hours to the current time");
                var result = await router.Prompt("now what time?");

                Assert.IsTrue(result is DateTime);
                Assert.AreEqual(time.AddHours(38).ToString(), result.ToString());
            });
        }*/

        /*public class PersonProvider {
            public IEnumerable<Person> Data { get; set; }

            public PersonProvider(IEnumerable<Person> data) {
                Data = data;
            }

            public IEnumerable<Person> ByState(string state) {
                Data = Data.Where(each => each.State == state);
                return Data;
            }

            public IEnumerable<Person> AgeRange(int min, int max) {
                Data = Data.Where(each => each.Age >= min && each.Age <= max);
                return Data;
            }

            public int Count() {
                return Data.Count();
            }
        }*/

        /*[TestMethod]
        public async Task ShouldInvokeRoute_LINQ() {
            await With<IntentRouter>(async router => {

                var provider = new PersonProvider(new List<Person> {
                    new() {Age = 10, State = "Florida"},
                    new() {Age = 15, State = "Florida"},
                    new() {Age = 24, State = "Florida"},
                    new() {Age = 20, State = "Maryland"},
                    new() {Age = 25, State = "Maryland"}
                });

                router.Use(provider)
                    .Route("count", data => data.Count())
                    .Route("people living on the west coast", data => data.ByState("California"));

                var response = await router.Prompt("count the people that live in the sunshine state");
                Assert.AreEqual(3, response);
            });
        }*/

        /*[TestMethod]
        public async Task ShouldInvokeRoute_LINQ2() {
            await With<IntentRouter>(async router => {

                var provider = new PersonProvider(new List<Person> {
                    new() {Age = 10, State = "Florida"},
                    new() {Age = 13, State = "Florida"},
                    new() {Age = 15, State = "Florida"},
                    new() {Age = 34, State = "Florida"},
                    new() {Age = 20, State = "Maryland"},
                    new() {Age = 25, State = "Maryland"}
                });

                router.Use(provider)
                    .Route("count", data => data.Count())
                    .Route("people between the age of 50 and 70", data => data.AgeRange(50, 70))
                    .Route("people living on the west coast", data => data.ByState("California")); // Coerce entity from encoded knowledge

                var response = await router.Prompt("young children in the sunshine state");
            });
        }*/

        [TestMethod]
        public async Task ShouldInvoke_Complex_MultiIntent() {
            await With<IntentRouter>(async router => {

                var time = DateTime.Parse("1/1/2021 1:00 PM");
                
                router.Use(new EventListener())
                    .Route("when you see a red square", events => events.On("see a red square")
                );

                router.Use(new EventScheduler())
                    .Route("restart the computer in 10 minutes", scheduler => scheduler.Schedule("restart the computer", "10 minutes")
                );
                
                var result = await router.Prompt("count how many times I click the left mouse button over the next 5 seconds");
                
                //schedule(5).count(get('left click'))
            });
        }

        [TestMethod]
        public async Task ShouldAutomate_MouseCursor() {
            var router = TestHelper.GetMouseCursor();

            await router.Prompt("move the cursor in a square");
        }

        // Todo:  Need an 'intent sub-solver' for phrases like 'left mouse button'
    }
}