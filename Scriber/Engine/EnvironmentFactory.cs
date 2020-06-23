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
            object? InvokeDynamic(Element element, CompilerState state, Argument[] content, Argument[] args)
            {
                var embedded = Embed(args, content);
                var sorted = DynamicDispatch.SortArguments(command, element, state, embedded, param);
                DynamicDispatch.MatchArguments(state, sorted, param);
                return method.Invoke(null, sorted);
            }
        }

        private static Argument[] Embed(Argument[] parent, Argument[] child)
        {
            var array = new Argument[parent.Length + 1];

            array[0] = new Argument(new Element(null, ElementType.Block, 0, 0), child);

            for (int i = 0; i < parent.Length; i++)
            {
                array[i + 1] = parent[i];
            }

            return array;
        }
    }
}
