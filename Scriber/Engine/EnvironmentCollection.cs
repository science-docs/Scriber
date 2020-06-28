using System.Collections.Generic;

namespace Scriber.Engine
{
    public class EnvironmentCollection
    {
        private readonly Dictionary<string, Environment> environments = new Dictionary<string, Environment>();

        public void Add(Environment environment)
        {
            environments[environment.Name] = environment;
        }

        public Environment? Find(string name)
        {
            environments.TryGetValue(name, out var environment);
            return environment;
        }
    }
}
