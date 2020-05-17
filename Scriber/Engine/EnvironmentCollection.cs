using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Engine
{
    public static class EnvironmentCollection
    {
        private static readonly Dictionary<string, Environment> environments = new Dictionary<string, Environment>();

        public static void Add(Environment environment)
        {
            environments[environment.Name] = environment;
        }

        public static Environment? Find(string name)
        {
            environments.TryGetValue(name, out var environment);
            return environment;
        }
    }
}
