using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public abstract class EnvironmentInstruction : EngineInstruction
    {
        public bool IsEnd { get; set; }

        public override object Execute(CompilerState state, object[] args)
        {
            object result;

            if (IsEnd)
            {
                var objects = state.Environments.Current.Objects.ToArray();
                result = End(state, objects);
                state.Environments.Pop();
            }
            else
            {
                state.Environments.Push();
                result = Start(state, args);
            }
            return result;
        }

        public abstract object Start(CompilerState state, object[] args);
        public abstract object End(CompilerState state, object[] objects);

        public static new EnvironmentInstruction Create(Element element)
        {
            throw new NotImplementedException();
        }
    }
}
