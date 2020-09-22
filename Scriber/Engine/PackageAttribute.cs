using System;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PackageAttribute : Attribute
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public string? Example { get; set; }

        public PackageAttribute()
        {
        }

        public PackageAttribute(string name)
        {
            Name = name;
        }
    }
}
