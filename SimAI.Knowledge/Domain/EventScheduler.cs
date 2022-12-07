using System;
using System.Collections.Generic;

namespace SimAI.Knowledge.Domain {
    public class EventScheduler {
        public List<string> EventQueue = new();

        public void Schedule(string action, string time) {
            EventQueue.Add(action + " " + time);
        }
    }
}