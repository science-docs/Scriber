using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ObjectFieldAttribute : Attribute
    {
        public string Name { get; }
        public string? Description { get; set; }

        public ObjectFieldAttribute(string name)
        {
            Name = name;
        }
    }
}
