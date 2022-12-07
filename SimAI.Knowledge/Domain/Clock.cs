using System;

namespace SimAI.Knowledge.Domain {
    public class Clock {

        private DateTime _time;

        public Clock(DateTime time) {
            _time = time;
        }

        public Clock()
            : this(DateTime.Now) {
        }

        public DateTime GetTime() {
            return _time;
        }

        public void SetTime(DateTime time) {
            _time = time;
        }

        public void AddTime(TimeSpan time) {
            _time = _time.Add(time);
        }
    }
}
