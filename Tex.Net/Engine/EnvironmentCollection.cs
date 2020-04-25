using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public static class EnvironmentCollection
    {
        private static readonly Dictionary<string, EnvironmentInstance> environments = new Dictionary<string, EnvironmentInstance>();

        public static void Add(EnvironmentInstance environment)
        {
            environments[environment.Name] = environment;
        }

        public static EnvironmentInstance Find(string name)
        {
            environments.TryGetValue(name, out var environment);
            return environment;
        }
    }
}
