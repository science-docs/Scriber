using System;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class EnvironmentAttribute : Attribute
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public string? Example { get; set; }

        public EnvironmentAttribute(string name)
        {
            Name = name;
        }
    }
}
