using Scriber.Language;
using System.Reflection;

namespace Scriber.Engine
{
    public static class EnvironmentFactory
    {
        public static Environment Create(EnvironmentAttribute attribute, MethodInfo info)
        {
            var invoke = CreateDelegate(attribute.Name, info, out var parameters);
            var environmentInstance = new Environment(attribute.Name, invoke, parameters);
            return environmentInstance;
        }

        private static EnvironmentExecution CreateDelegate(string command, MethodInfo method, out ParameterInfo[] parameters)
        {
            var param = method.GetParameters();
            parameters = param;

            return InvokeDynamic;

            // inline method for better debugging
            object? InvokeDynamic(Element element, CompilerState state, object?[] content, object?[] args)
            {
                var embedded = Embed(args, content);
                var sorted = DynamicDispatch.SortArguments(command, element, state, embedded, param);
                DynamicDispatch.MatchArguments(state, element, sorted, param);
                return method.Invoke(null, sorted);
            }
        }

        private static object?[] Embed(object?[] parent, object?[] child)
        {
            var array = new object?[parent.Length + 1];

            array[0] = child;

            for (int i = 0; i < parent.Length; i++)
            {
                array[i + 1] = parent[i];
            }

            return array;
        }
    }
}
