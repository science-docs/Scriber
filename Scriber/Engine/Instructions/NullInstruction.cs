using Scriber.Language;
using System;

namespace Scriber.Engine.Instructions
{
    public class NullInstruction : EngineInstruction
    {
        public static object NullObject { get; } = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        public NullInstruction(Element origin) : base(origin)
        {
        }

        public override object? Execute(CompilerState state, Argument[] arguments)
        {
            return NullObject;
        }
    }
}
