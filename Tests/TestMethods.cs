using ExpressionEvaluator;
using System;

namespace Tests
{
    class TestType
    {
        public string Name { get; set; }
    }

    class TestMethods
    {
        [Function]
        static TestType TestFunc(TestType param1, TestType param2)
        {
            return new TestType { Name = param1.Name + param2.Name };
        }

        [Function(InjectContext = true)]
        static string GetUsername(string context)
        {
            return "jacob-" + context;
        }
    }
}
