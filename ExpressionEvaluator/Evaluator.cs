using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpressionEvaluator
{
    public class Evaluator
    {
        private Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();

        public Evaluator(params Assembly[] customAssemblies)
        {
            var defaultMethods = typeof(Evaluator).GetTypeInfo().Assembly
                .DefinedTypes
                .SelectMany(t => t.DeclaredMethods)
                .Where(m => m.GetCustomAttributes(typeof(FunctionAttribute), false).Any());

            LoadFromAssembly(typeof(Evaluator).GetTypeInfo().Assembly);

            if (customAssemblies?.Any() ?? false)
                foreach (var assembly in customAssemblies)
                    LoadFromAssembly(assembly);

            if (!_methods.Any())
            {
                throw new Exception("No functions found in the provided assemblies.");
            }
        }

        void LoadFromAssembly(Assembly assembly)
        {
            var methods = assembly
                ?.DefinedTypes
                .SelectMany(t => t.DeclaredMethods)
                .Where(m =>
                    m.IsStatic &&
                    m.GetCustomAttributes(typeof(FunctionAttribute), false).Any());

            foreach (var method in methods)
            {
                string name = method.GetCustomAttribute<FunctionAttribute>().Name ?? method.Name;
                _methods[name] = method;
            }
        }

        public object Evaluate(Expression expression, string context = null)
        {
            if (expression.Function != null)
            {
                if (!_methods.ContainsKey(expression.Function))
                {
                    throw new Exception($"Function {expression.Function} is not supported.");
                }

                MethodInfo method = _methods[expression.Function];

                object[] param = GetParams(method.GetParameters(), expression.Params?.ToArray(), context)?.Cast<object>().ToArray();

                if (method.GetCustomAttribute<FunctionAttribute>().InjectContext)
                {
                    param = new object[] { context }.Concat(param).ToArray();
                }

                return method.Invoke(null, param);
            }

            return expression;
        }

        private IEnumerable GetParams(ParameterInfo[] parameterInfo, Expression[] @params, string context)
        {
            if (@params == null) return Enumerable.Empty<object>();

            Type[] paramTypes = null;
            if (parameterInfo.Length == @params.Length)
            {
                paramTypes = parameterInfo.Select(p => p.ParameterType).ToArray();
            }
            else if (parameterInfo.Length == 1)
            {
                paramTypes = @params.Select(e => parameterInfo[0].ParameterType.GetElementType()).ToArray();
            }
            else
            {
                throw new Exception("Invalid params");
            }

            object[] result = new object[@params.Length];
            for (int i = 0; i < @params.Length; i++)
            {
                if (@params[i].Function != null)
                {
                    result[i] = Evaluate(@params[i], context);
                }
                else
                {
                    result[i] = JsonConvert.DeserializeObject(
                        JsonConvert.SerializeObject(@params[i].Literal),
                        paramTypes[i]);
                }
            }

            if (parameterInfo.Length == @params.Length)
            {
                return result;
            }

            var arr = Array.CreateInstance(paramTypes[0], result.Length);
            for (int i = 0; i < result.Length; i++)
                arr.SetValue(result[i], i);
            return new object[] { arr };
        }
    }
}
