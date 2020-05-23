using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Scriber.Engine
{
    public static class ReflectionLoader
    {
        private static readonly List<Assembly> assemblies = new List<Assembly>();

        static ReflectionLoader()
        {
            Add(typeof(ReflectionLoader).Assembly);
        }

        public static void Add(Assembly asm)
        {
            lock (assemblies)
            {
                assemblies.Add(asm);
            }
        }

        public static void Discover(params Assembly[] additionalAssemblies)
        {
            lock (assemblies)
            {
                assemblies.AddRange(additionalAssemblies);

                foreach (var asm in assemblies)
                {
                    DiscoverAssembly(asm);
                }

                // After assemblies have been discovered, remove them
                assemblies.Clear();
            }
        }

        private static void DiscoverAssembly(Assembly asm)
        {
            var packages = asm.GetTypes().Where(e => IsPackage(e));

            foreach (var package in packages)
            {
                FindCommands(package);
                FindEnvironments(package);
            }

            var converters = asm.GetTypes().Where(e => IsConverter(e));

            foreach (var converter in converters)
            {
                FindConverter(converter);
            }
        }

        private static bool IsPackage(Type type)
        {
            return type.GetCustomAttribute<PackageAttribute>() != null;
        }

        private static bool IsConverter(Type type)
        {
            return type.GetCustomAttribute<CommandArgumentConverterAttribute>() != null;
        }

        private static void FindCommands(Type type)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (IsCommand(method, out var command) && command != null)
                {
                    CommandCollection.Add(CommandFactory.Create(command, method));
                }
            }
        }

        private static bool IsCommand(MethodInfo method, out CommandAttribute? command)
        {
            command = method.GetCustomAttribute<CommandAttribute>();
            return command != null && method.IsStatic;
        }

        private static void FindEnvironments(Type type)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (IsEnvironment(method, out var environment) && environment != null)
                {
                    EnvironmentCollection.Add(EnvironmentFactory.Create(environment, method));
                }
            }
        }

        private static bool IsEnvironment(MethodInfo method, out EnvironmentAttribute? environment)
        {
            var objArray = new Type[] { typeof(object[]) };
            var stateObjArray = new Type[] { typeof(CompilerState), typeof(object[]) };
            environment = method.GetCustomAttribute<EnvironmentAttribute>();
            return environment != null && method.IsStatic && MatchesArgs(method, objArray, stateObjArray);
        }

        private static void FindConverter(Type type)
        {
            var attribute = type.GetCustomAttribute<CommandArgumentConverterAttribute>();

            if (attribute != null && typeof(IElementConverter).IsAssignableFrom(type))
            {
                if (!(Activator.CreateInstance(type) is IElementConverter instance))
                {
                    throw new Exception("Could not create converter instance from type " + type.Name);
                }

                ElementConverters.Add(instance, attribute.Source, attribute.Targets);
            }
        }

        private static bool MatchesArgs(MethodInfo method, params Type[][] types)
        {
            var args = method.GetParameters();
            foreach (var typeList in types)
            {
                bool matches = true;
                if (args.Length >= typeList.Length)
                {
                    for (int i = 0; i < typeList.Length; i++)
                    {
                        if (args[i].ParameterType != typeList[i])
                        {
                            matches = false;
                            break;
                        }
                    }
                }

                if (matches)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
