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
        /// <param name="element"></param>
        /// <exception cref="ArgumentNullException"/>
        public ObjectFieldInstruction(Element element) : base(element)
        {
            Key = element.Content ?? throw new ArgumentNullException($"{nameof(element)}.{nameof(element.Content)}");
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
            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (arguments.Length != 1)
            {
                throw new ArgumentException("An object field can only hold one argument.", nameof(arguments));
            }

            return new ObjectField(Origin, Key, arguments[0]);
        }
    }
}
