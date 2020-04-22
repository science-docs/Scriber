using System;

namespace Tex.Net.Engine
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandAttribute : Attribute
    {
        public string Name { get; set; }
        public string RequiredEnvironment { get; set; }

        public CommandAttribute(string name)
        {
            Name = name;
        }
    }
}
