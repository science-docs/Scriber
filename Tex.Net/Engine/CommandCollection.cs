using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine
{
    public static class CommandCollection
    {
        private static readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();
        private static readonly List<Assembly> assemblies = new List<Assembly>();

        static CommandCollection()
        {
            Add(typeof(CommandCollection).Assembly);
        }

        public static void Add(Assembly asm)
        {
            lock (assemblies)
            {
                assemblies.Add(asm);
            }
        }

        public static void Add(Command command)
        {
            commands[command.Name] = command;
        }

        public static Command Find(string name)
        {
            commands.TryGetValue(name, out var command);
            return command;
        }

        public static void Discover(params Assembly[] additionalAssemblies)
        {
            lock (assemblies)
            {
                assemblies.AddRange(additionalAssemblies);

                foreach (var asm in assemblies)
                {
                    var types = asm.GetTypes().Where(e => IsPackage(e));

                    foreach (var type in types)
                    {
                        FindCommands(type);
                    }
                }

                // After assemblies have been discovered, remove them
                assemblies.Clear();
            }
        }

        private static bool IsPackage(Type type)
        {
            return type.GetCustomAttribute<PackageAttribute>() != null;
        }

        private static void FindCommands(Type type)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (IsCommand(method, out var command))
                {
                    var del = CreateDelegate(command.Name, method, out var parameters);
                    var commandItem = new Command
                    {
                        Name = command.Name,
                        Execution = del,
                        Parameters = parameters,
                        RequiredEnvironment = command.RequiredEnvironment
                    };
                    Add(commandItem);
                }
            }
        }

        private static bool IsCommand(MethodInfo method, out CommandAttribute command)
        {
            command = method.GetCustomAttribute<CommandAttribute>();
            return command != null && method.IsStatic;
        }

        private static CommandExecution CreateDelegate(string command, MethodInfo method, out ParameterInfo[] parameters)
        {
            var param = method.GetParameters();
            parameters = param;

            return (state, args) =>
            {
                var sorted = SortArguments(command, state, args, param);
                MatchArguments(sorted, param);
                return method.Invoke(null, sorted);
            };
        }

        private static object[] SortArguments(string command, CompilerState state, object[] args, ParameterInfo[] parameters)
        {
            if (parameters.Length == 0)
            {
                if (args.Length != 0)
                {
                    throw new CommandInvocationException($"Command {command} does not expect any arguments. Received: {args.Length}");
                }
                return Array.Empty<object>();
            }

            List<object> objects = new List<object>();

            CountParameters(parameters, out var hasState, out int required, out int optional);

            if (hasState)
            {
                objects.Add(state);
            }

            if (args.Length < required || args.Length > required + optional)
            {
                throw new CommandInvocationException($"Command {command} expects between {required} and {optional} arguments. {args.Length} arguments where supplied");
            }

            var suppliedOptional = args.Length - required;

            objects.AddRange(args[suppliedOptional..^0]);
            objects.AddRange(args[0..suppliedOptional]);

            return objects.ToArray();
        }

        private static void MatchArguments(object[] args, ParameterInfo[] parameters)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var parm = parameters[i];
                if (!IsAssignableFrom(parm.ParameterType, arg, out var transformed))
                {
                    throw new CommandInvocationException($"Object of type {arg.GetType().Name} cannot be assigned or transformed to parameter of type {parm.ParameterType.Name}");
                }
                else
                {
                    args[i] = transformed;
                }
            }
        }

        private static bool IsAssignableFrom(Type target, object obj, out object transformed)
        {
            if (target.IsAssignableFrom(obj.GetType()))
            {
                transformed = obj;
                return true;
            }
            // todo: refactor this completely
            else if (target == typeof(IEnumerable<Leaf>))
            {
                if (obj is Paragraph paragraph)
                {
                    transformed = paragraph.Leaves;
                    return true;
                }
            }
            else if (target == typeof(Paragraph))
            {
                if (obj is Leaf leaf)
                {
                    var paragraph = new Paragraph();
                    paragraph.Leaves.Add(leaf);
                    transformed = paragraph;
                    return true;
                }
            }
            transformed = null;
            return false;
        }

        private static void CountParameters(ParameterInfo[] parameters, out bool hasState, out int required, out int optional)
        {
            int start = 0;
            required = 0;
            optional = 0;

            hasState = parameters[0].ParameterType == typeof(CompilerState);

            if (hasState)
            {
                start = 1;
            }

            for (int i = start; i < parameters.Length; i++)
            {
                if (parameters[i].IsOptional)
                {
                    optional++;
                }
                else
                {
                    required++;
                }
            }
        }
    }
}
