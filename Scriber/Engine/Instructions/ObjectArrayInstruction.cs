using Scriber.Language.Syntax;
using System;
using System.Collections.Generic;

namespace Scriber.Engine.Instructions
{
    public class ObjectArrayInstruction : EngineInstruction<ArraySyntax>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public override object? Execute(CompilerState state, ArraySyntax array)
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

            foreach (var arrayElement in array.Children)
            {
                arguments.Add(EngineInstruction.Execute(state, arrayElement));
            }

            return new ObjectArray(array, state, arguments.ToArray());
        }
    }
}
