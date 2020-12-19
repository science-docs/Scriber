using System;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class VariableAttribute : Attribute
    {
        public string Name { get; set; }

        public VariableAttribute(string name)
        {
            Name = name;
        }
    }
}
