using System;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ObjectFieldAttribute : Attribute
    {
        public string Name { get; }
        public string? Description { get; set; }
        public Type[]? Overrides { get; set; }

        public ObjectFieldAttribute(string name)
        {
            Name = name;
        }
    }
}
