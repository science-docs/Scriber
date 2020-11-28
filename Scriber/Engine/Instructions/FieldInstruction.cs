using System;
using Scriber.Language.Syntax;

namespace Scriber.Engine.Instructions
{
    public class FieldInstruction : EngineInstruction<FieldSyntax>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public override object? Evaluate(CompilerState state, FieldSyntax field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (field.Name is null)
            {
                throw new ArgumentException("Field name cannot be empty", nameof(field));
            }

            var fieldArgument = EngineInstruction.Evaluate(state, field.Value);

            return new ObjectField(field, field.Name.Value, fieldArgument);
        }
    }
}
