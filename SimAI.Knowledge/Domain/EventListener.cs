using System;
using System.Collections.Generic;

namespace SimAI.Knowledge.Domain {
    public class EventListener {
        public List<string> EventQueue = new();

        public void On(string condition) {
            EventQueue.Add(condition);
        }
    }
}