using System;
using System.Collections.Generic;
using System.Reflection;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine
{
    public static class CommandFactory
    {
        public static Command Create(CommandAttribute attribute, MethodInfo info)
        {
            var invoke = CreateDelegate(attribute.Name, info, out var parameters);
            var commandItem = new Command(attribute.Name, attribute.RequiredEnvironment, invoke, parameters);
            return commandItem;
        }

        private static CommandExecution CreateDelegate(string command, MethodInfo method, out ParameterInfo[] parameters)
        {
            var param = method.GetParameters();
            parameters = param;

            return InvokeDynamic;

            // inline method for better debugging
            object? InvokeDynamic(CompilerState state, object[] args)
            {
                var sorted = DynamicDispatch.SortArguments(command, state, args, param);
                DynamicDispatch.MatchArguments(state, sorted, param);
                return method.Invoke(null, sorted);
            }
        }
    }
}
