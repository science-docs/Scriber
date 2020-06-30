using Scriber.Language;
using System;

namespace Scriber.Engine.Instructions
{
    public class ObjectArrayInstruction : EngineInstruction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        public ObjectArrayInstruction(Element origin) : base(origin)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public override object? Execute(CompilerState state, Argument[] arguments)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            return new ObjectArray(Origin, state, arguments);
        }
    }
}
