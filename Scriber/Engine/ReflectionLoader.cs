using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scriber.Engine
{
    public class ReflectionLoader
    {
        private readonly HashSet<Assembly> assemblies = new HashSet<Assembly>();

        public ReflectionLoader()
        {
            Add(typeof(ReflectionLoader).Assembly);
        }

        public void Add(Assembly asm)
        {
            lock (assemblies)
            {
                assemblies.Add(asm);
            }
        }

        public void Discover(Context context, params Assembly[] additionalAssemblies)
        {
            lock (assemblies)
            {
                foreach (var asm in additionalAssemblies)
                {
                    assemblies.Add(asm);
                }

                foreach (var asm in assemblies)
                {
                    DiscoverAssembly(context, asm);
                }

                // After assemblies have been discovered, remove them
                assemblies.Clear();
            }
        }

        private void DiscoverAssembly(Context context, Assembly asm)
        {
            var packages = asm.GetTypes().Where(e => IsPackage(e));

            foreach (var package in packages)
            {
                InitializePackage(context, package);
            }

            foreach (var package in packages)
            {
                FindCommands(context, package);
                FindEnvironments(context, package);
            }

            var converters = asm.GetTypes().Where(e => IsConverter(e));

            foreach (var converter in converters)
            {
                FindConverter(context, converter);
            }
            context.Converters.ResolvePaths();
        }

        private void InitializePackage(Context context, Type type)
        {
            if (type.IsSubclassOf(typeof(IPackage)))
            {
                if (Activator.CreateInstance(type) is IPackage package)
                {
                    package.Initialize(context, this);
                }
            }
        }

        private bool IsPackage(Type type)
        {
            return type.GetCustomAttribute<PackageAttribute>() != null;
        }

        private bool IsConverter(Type type)
        {
            return type.GetCustomAttribute<CommandArgumentConverterAttribute>() != null;
        }

        private void FindCommands(Context context, Type type)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (IsCommand(method, out var command) && command != null)
                {
                    context.Commands.Add(CommandFactory.Create(command, method));
                }
            }
        }

        private bool IsCommand(MethodInfo method, out CommandAttribute? command)
        {
            command = method.GetCustomAttribute<CommandAttribute>();
            return command != null && method.IsStatic;
        }

        private void FindEnvironments(Context context, Type type)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (IsEnvironment(method, out var environment) && environment != null)
                {
                    context.Environments.Add(EnvironmentFactory.Create(environment, method));
                }
            }
        }

        private bool IsEnvironment(MethodInfo method, out EnvironmentAttribute? environment)
        {
            var argumentArray = new Type[] { typeof(Argument[]) };
            var objArray = new Type[] { typeof(object[]) };
            var stateObjArray = new Type[] { typeof(CompilerState), typeof(object[]) };
            var stateArgumentArray = new Type[] { typeof(CompilerState), typeof(Argument[]) };

            environment = method.GetCustomAttribute<EnvironmentAttribute>();
            return environment != null && method.IsStatic && MatchesArgs(method, argumentArray, objArray, stateObjArray, stateArgumentArray);
        }

        private void FindConverter(Context context, Type type)
        {
            var attribute = type.GetCustomAttribute<CommandArgumentConverterAttribute>();

            if (attribute != null)
            {
                if (typeof(IElementConverter).IsAssignableFrom(type) &&
                    Activator.CreateInstance(type) is IElementConverter instance)
                {
                    context.Converters.Add(instance, attribute.Source, attribute.Targets);
                }
                else
                {
                    context.Logger.Warning($"Could not create converter instance from type '{type.FormattedName()}'.");
                }
            }
        }

        private bool MatchesArgs(MethodInfo method, params Type[][] types)
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
