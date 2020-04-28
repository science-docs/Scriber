using System.Reflection;

namespace Tex.Net.Engine
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
            object? InvokeDynamic(CompilerState state, object[] content, object[] args)
            {
                var embedded = Embed(args, content);
                var sorted = DynamicDispatch.SortArguments(command, state, embedded, param);
                DynamicDispatch.MatchArguments(state, sorted, param);
                return method.Invoke(null, sorted);
            }
        }

        private static object[] Embed(object[] parent, object[] child)
        {
            parent[0] = child;
            return parent;
        }
    }
}
