using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ObjectFieldAttribute : Attribute
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
