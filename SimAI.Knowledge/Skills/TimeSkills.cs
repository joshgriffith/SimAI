using System;
using SimAI.Core.Intent;
using SimAI.Core.Skills;

namespace SimAI.Knowledge.Skills {

    [Skill("time")]
    public class TimeSkills {

        private DateTime _time;

        public TimeSkills() {
            _time = DateTime.Now;
        }

        [Intent("get time")]
        [Sample("what time is it", "time", "what is the time", "get the time")]
        public DateTime GetTime() {
            return _time;
        }

        [Intent("add time", "subtract time", "move time forward", "move time backward")]
        [Sample("{number} {time unit} pass")]
        public DateTime ChangeTime(TimeSpan time) {
            _time = _time.Add(time);
            return _time;
        }

        [Intent("set time", "change time")]
        [Sample("the time is {time}")]
        public DateTime SetTime(DateTime time) {
            _time = time;
            return _time;
        }
    }
}