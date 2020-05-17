using System;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class EnvironmentAttribute : Attribute
    {
        public string Name { get; set; }

        public EnvironmentAttribute(string name)
        {
            Name = name;
        }
    }
}
