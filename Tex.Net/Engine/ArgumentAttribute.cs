using System;

namespace Tex.Net.Engine
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ArgumentAttribute : Attribute
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Type[]? Overrides { get; set; }
    }
}
