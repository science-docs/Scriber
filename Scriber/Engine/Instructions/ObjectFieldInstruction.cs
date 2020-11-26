using System;
using Scriber.Language;
using Scriber.Language.Syntax;

namespace Scriber.Engine.Instructions
{
    public class ObjectFieldInstruction : EngineInstruction<FieldSyntax>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public override object? Execute(CompilerState state, FieldSyntax field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (field.Name is null)
            {
                throw new ArgumentException();
            }

            var fieldArgument = EngineInstruction.Execute(state, field.Value);

            return new ObjectField(field, field.Name.Value, fieldArgument);
        }
    }
}
