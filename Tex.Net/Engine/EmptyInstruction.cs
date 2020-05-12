using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public class EmptyInstruction : EngineInstruction
    {
        public static object Object { get; } = new object();

        public override object? Execute(CompilerState state, object[] arguments)
        {
            return Object;
        }
    }
}
