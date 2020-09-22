using System;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ObjectTypeAttribute : Attribute
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public string? Example { get; set; }

        public ObjectTypeAttribute()
        {

        }

        public ObjectTypeAttribute(string name)
        {
            Name = name;
        }
    }
}
