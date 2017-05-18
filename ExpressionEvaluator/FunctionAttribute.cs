using System;

namespace ExpressionEvaluator
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class FunctionAttribute : Attribute
    {
        private string _name;

        public FunctionAttribute(string name = null)
        {
            _name = name;
        }

        public string Name { get { return _name; } }

        public bool InjectContext { get; set; }
    }
}
