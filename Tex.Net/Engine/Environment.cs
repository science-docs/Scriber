using System.Collections.Generic;

namespace Tex.Net.Engine
{
    public class Environment
    {
        /// <summary>
        /// The name of the environment.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List of objects that the current environment contains.
        /// </summary>
        public List<object> Objects { get; } = new List<object>();
        /// <summary>
        /// List of arguments that where supplied to the environment begin command.
        /// </summary>
        public List<object> Arguments { get; } = new List<object>();
        public EnvironmentInstance Instance { get; internal set; }

        public Environment(string name)
        {
            Name = name;
        }
    }
}
