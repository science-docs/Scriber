using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
            assemblies.Add(asm);
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

        public static void Discover()
        {
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
                        Parameters = parameters
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
                if (args.Length == 0)
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
                if (!parm.ParameterType.IsAssignableFrom(arg.GetType()))
                {
                    throw new CommandInvocationException($"Object of type {arg.GetType().Name} cannot be assigned to parameter of type {parm.ParameterType.Name}");
                }
            }
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
