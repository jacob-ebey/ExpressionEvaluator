using System.Linq;

namespace ExpressionEvaluator
{
    class DefaultFunctions
    {
        [Function("if")]
        public static object If(bool check, object trueResult, object falseResult)
        {
            return check ? trueResult : falseResult;
        }

        [Function("+")]
        public static double Add(params double[] param)
        {
            double result = param[0];
            foreach (var d in param.Skip(1)) result += d;
            return result;
        }

        [Function("-")]
        public static double Subtract(params double[] param)
        {
            double result = param[0];
            foreach (var d in param.Skip(1)) result -= d;
            return result;
        }

        [Function("*")]
        public static double Multiply(params double[] param)
        {
            double result = param[0];
            foreach (var d in param.Skip(1)) result *= d;
            return result;
        }

        [Function("/")]
        public static double DivideBy(double param1, double param2)
        {
            return param1 / param2;
        }

        [Function("<")]
        public static bool LessThan(double param1, double param2)
        {
            return param1 < param2;
        }

        [Function(">")]
        public static bool GreaterThan(double param1, double param2)
        {
            return param1 > param2;
        }

        [Function("=")]
        public static bool ObjectEquals(object param1, object param2)
        {
            return param1.Equals(param2);
        }
    }
}
