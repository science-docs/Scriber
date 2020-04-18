using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public abstract class EnvironmentInstruction : EngineInstruction
    {
        public bool IsEnd { get; set; }

        public override object Execute(CompilerState state)
        {
            return IsEnd ? End(state) : Start(state);
        }

        public abstract object Start(CompilerState state);
        public abstract object End(CompilerState state);
    }
}
