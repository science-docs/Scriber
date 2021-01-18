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
            return type.GetCustomAttribute<ConverterAttribute>() != null;
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

        private void FindConverter(Context context, Type type)
        {
            var attribute = type.GetCustomAttribute<ConverterAttribute>();

            if (attribute != null)
            {
                if (typeof(IConverter).IsAssignableFrom(type) &&
                    Activator.CreateInstance(type) is IConverter instance)
                {
                    context.Converters.Add(instance, attribute.Source, attribute.Targets);
                }
                else
                {
                    context.Logger.Warning($"Could not create converter instance from type '{type.FormattedName()}'.");
                }
            }
        }
    }
}
