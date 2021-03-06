﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConverterAttribute : Attribute
    {
        public Type Source { get; }
        public Type[] Targets { get; }

        public ConverterAttribute(Type source, params Type[] targets)
        {
            Source = source;
            Targets = targets;
        }
    }
}
