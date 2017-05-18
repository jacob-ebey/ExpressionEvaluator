using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpressionEvaluator;
using System;
using System.Diagnostics;

namespace Tests
{
    [TestClass]
    public class EvaluatorTests
    {
        private Evaluator _evaluator;

        [TestInitialize]
        public void Setup()
        {
            _evaluator = new Evaluator();
        }
        
        [TestMethod]
        public void Add()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "+",
                Params = new[]
                {
                    new Expression { Literal = 1 },
                    new Expression { Literal = 2 },
                    new Expression { Literal = 3 },
                }
            });
            
            Assert.AreEqual(6.0, result);
        }

        [TestMethod]
        public void Subtract()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "-",
                Params = new[]
                {
                    new Expression { Literal = 1 },
                    new Expression { Literal = 2 },
                    new Expression { Literal = 3 },
                }
            });

            Assert.AreEqual(-4.0, result);
        }

        [TestMethod]
        public void Nested()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "+",
                Params = new[]
                {
                    new Expression { Literal = 2 },
                    new Expression
                    {
                        Function = "-",
                        Params = new[]
                        {
                            new Expression { Literal = 10 },
                            new Expression { Literal = 7 }
                        }
                    }
                }
            });

            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void EqualsBool()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "=",
                Params = new[]
                {
                    new Expression { Literal = true },
                    new Expression { Literal = true }
                }
            });

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void NotEqualsBool()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "=",
                Params = new[]
                {
                    new Expression { Literal = true },
                    new Expression { Literal = false }
                }
            });

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void EqualsString()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "=",
                Params = new[]
                {
                    new Expression { Literal = "test" },
                    new Expression { Literal = "test" }
                }
            });

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void EqualsNotSameTypes()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "=",
                Params = new[]
                {
                    new Expression { Literal = "test" },
                    new Expression { Literal = 2.0 }
                }
            });

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void IfLiteralTrue()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "if",
                Params = new[]
                {
                    new Expression { Literal = true },
                    new Expression { Literal = "true" },
                    new Expression { Literal = "false" }
                }
            });

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void IfLiteralFalse()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "if",
                Params = new[]
                {
                    new Expression { Literal = false },
                    new Expression { Literal = "true" },
                    new Expression { Literal = "false" }
                }
            });

            Assert.AreEqual("false", result);
        }

        [TestMethod]
        public void IfExpressionTrue()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "if",
                Params = new[]
                {
                    new Expression
                    {
                        Function = ">",
                        Params = new[]
                        {
                            new Expression { Literal = 2 },
                            new Expression { Literal = 1 },
                        }
                    },
                    new Expression { Literal = "true" },
                    new Expression { Literal = "false" }
                }
            });

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void IfExpressionFalse()
        {
            var result = _evaluator.Evaluate(new Expression
            {
                Function = "if",
                Params = new[]
                {
                    new Expression
                    {
                        Function = "<",
                        Params = new[]
                        {
                            new Expression { Literal = 2 },
                            new Expression { Literal = 1 },
                        }
                    },
                    new Expression { Literal = "true" },
                    new Expression { Literal = "false" }
                }
            });

            Assert.AreEqual("false", result);
        }

        [TestMethod]
        public void CustomMethod()
        {
            Evaluator evaluator = new Evaluator(typeof(EvaluatorTests).Assembly);
            var result = evaluator.Evaluate(new Expression
            {
                Function = "TestFunc",
                Params = new[]
                {
                    new Expression
                    {
                        Literal = new TestType
                        {
                            Name = "First"
                        }
                    },
                    new Expression
                    {
                        Literal = new TestType
                        {
                            Name = "Second"
                        }
                    }
                }
            });

            Assert.AreEqual("FirstSecond", (result as TestType)?.Name);
        }

        [TestMethod]
        public void FuncCanAccessContext()
        {
            Evaluator evaluator = new Evaluator(typeof(EvaluatorTests).Assembly);
            string context = Guid.NewGuid().ToString();
            var result = evaluator.Evaluate(new Expression
            {
                Function = "GetUsername",
            }, context);

            Assert.AreEqual($"jacob-{context}", result);
        }
    }
}
