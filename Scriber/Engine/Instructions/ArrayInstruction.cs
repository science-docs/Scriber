using Scriber.Language.Syntax;
using System;
using System.Collections.Generic;

namespace Scriber.Engine.Instructions
{
    public class ArrayInstruction : EngineInstruction<ArraySyntax>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public override object? Evaluate(CompilerState state, ArraySyntax array)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var arguments = new List<Argument>();

            foreach (var arrayElement in array)
            {
                arguments.Add(EngineInstruction.Evaluate(state, arrayElement));
            }

            return new ObjectArray(array, state, arguments.ToArray());
        }
    }
}
