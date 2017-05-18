using System.Collections.Generic;

namespace ExpressionEvaluator
{
    public class Expression
    {
        public string Function { get; set; }

        public IEnumerable<Expression> Params { get; set; }

        public object Literal { get; set; }
    }
}
