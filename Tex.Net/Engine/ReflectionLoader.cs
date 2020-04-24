﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tex.Net.Engine
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
                if (IsCommand(method, out var command))
                {
                    CommandCollection.Add(CommandFactory.Create(command, method));
                }
            }
        }

        private static bool IsCommand(MethodInfo method, out CommandAttribute command)
        {
            command = method.GetCustomAttribute<CommandAttribute>();
            return command != null && method.IsStatic;
        }

        private static void FindConverter(Type type)
        {
            var attribute = type.GetCustomAttribute<CommandArgumentConverterAttribute>();

            if (typeof(IElementConverter).IsAssignableFrom(type))
            {
                var instance = Activator.CreateInstance(type) as IElementConverter;
                ElementConverters.Add(instance, attribute.Source, attribute.Targets);
            }
        }
    }
}