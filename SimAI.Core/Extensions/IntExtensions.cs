using System;

namespace SimAI.Core.Extensions {
    public static class IntExtensions {

        public static void ForEach(this int count, Action action) {
            for (int index = 0; index < count; index++) {
                action();
            }
        }

        public static void ForEach(this int count, Action<int> action) {
            for (int index = 0; index < count; index++) {
                action(index);
            }
        }

        public static bool IsEven(this int value) {
            return value % 2 == 0;
        }

        public static bool IsOdd(this int value) {
            return value % 2 != 0;
        }
    }
}