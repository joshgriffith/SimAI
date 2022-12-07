namespace SimAI.Core.Extensions {
    public static class MathExtensions {

        public static double Sigmoid(this double input) {
            return 1/(1 + System.Math.Exp(-input));
        }
    }
}
