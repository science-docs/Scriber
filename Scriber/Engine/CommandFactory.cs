using Scriber.Language;
using Scriber.Language.Syntax;
using System.Linq;
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

        private static CommandExecution CreateDelegate(string command, MethodInfo method, out Parameter[] parameters)
        {
            var param = method.GetParameters();
            var items = new Parameter[param.Length];
            for (int i = 0; i < param.Length; i++)
            {
                items[i] = new Parameter(param[i]);
            }

            parameters = items;
            if (items.Length > 0 && items[0].Type == typeof(CompilerState))
            {
                parameters = items.Skip(1).ToArray();
            }

            return InvokeDynamic;

            // inline method for better debugging
            object? InvokeDynamic(SyntaxNode element, CompilerState state, Argument[] args)
            {
                var paddedArgs = DynamicDispatch.PadArguments(command, element, state, args, items);
                var matchedArgs = DynamicDispatch.MatchArguments(state, paddedArgs, items);
                try
                {
                    return method.Invoke(null, matchedArgs);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is CompilerException compilerException)
                    {
                        throw compilerException;
                    }
                    else if (ex.InnerException != null)
                    {
                        throw new CompilerException(element, ex.InnerException.Message, ex.InnerException);
                    }
                    else
                    {
                        throw new CompilerException(element, ex.Message, ex);
                    }
                }
            }
        }
    }
}
