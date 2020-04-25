using System.Reflection;

namespace Tex.Net.Engine
{
    public static class EnvironmentFactory
    {
        public static Environment Create(EnvironmentAttribute attribute, MethodInfo info)
        {
            var invoke = CreateDelegate(attribute.Name, info, out var parameters);
            var environmentInstance = new Environment
            {
                Name = attribute.Name,
                Execution = invoke,
                Parameters = parameters
            };
            return environmentInstance;
        }

        private static EnvironmentExecution CreateDelegate(string command, MethodInfo method, out ParameterInfo[] parameters)
        {
            var param = method.GetParameters();
            parameters = param;

            return InvokeDynamic;

            // inline method for better debugging
            object InvokeDynamic(CompilerState state, object[] content, object[] args)
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
            //var arr = new object[parent.Length + 1];
            //arr[0] = child;
            //for (int i = 0; i < parent.Length; i++)
            //{
            //    arr[i + 1] = parent[i];
            //}
            //return arr;
        }
    }
}
