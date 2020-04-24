using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandArgumentConverterAttribute : Attribute
    {
        public Type Source { get; }
        public Type[] Targets { get; }

        public CommandArgumentConverterAttribute(Type source, params Type[] targets)
        {
            Source = source;
            Targets = targets;
        }
    }
}
