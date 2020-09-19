using Scriber.Language;
using System.Reflection;

namespace Scriber.Engine
{
    public static class CommandFactory
    {
        public static Command Create(CommandAttribute attribute, MethodInfo info)
        {
            var invoke = CreateDelegate(attribute.Name, info, out var parameters);
            var commandItem = new Command(attribute.Name, invoke, parameters);
            return commandItem;
        }

        private static CommandExecution CreateDelegate(string command, MethodInfo method, out ParameterInfo[] parameters)
        {
            var param = method.GetParameters();
            parameters = param;

            return InvokeDynamic;

            // inline method for better debugging
            object? InvokeDynamic(Element element, CompilerState state, Argument[] args)
            {
                var sorted = DynamicDispatch.PadArguments(command, element, state, args, param);
                DynamicDispatch.MatchArguments(state, sorted, param);
                return method.Invoke(null, sorted);
            }
        }
    }
}
