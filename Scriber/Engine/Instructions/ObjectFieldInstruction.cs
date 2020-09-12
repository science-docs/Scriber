using System;
using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class ObjectFieldInstruction : EngineInstruction
    {
        public string Key { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public ObjectFieldInstruction(Element origin) : base(origin)
        {
            Key = origin.Content ?? throw new ArgumentException("Element content should not be null", nameof(origin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public override object? Execute(CompilerState state, Argument[] arguments)
        {
            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (arguments.Length != 1)
            {
                throw new ArgumentException("An object field can only hold one argument.", nameof(arguments));
            }

            if (arguments[0] == null)
            {
                throw new ArgumentException("Null arguments are not allowed", nameof(arguments));
            }

            return new ObjectField(Origin, Key, arguments[0]);
        }
    }
}
