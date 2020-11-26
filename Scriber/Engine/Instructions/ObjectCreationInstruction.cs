using System;
using System.Collections.Generic;
using Scriber.Language.Syntax;

namespace Scriber.Engine.Instructions
{
    public class ObjectCreationInstruction : EngineInstruction<ObjectSyntax>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public override object? Execute(CompilerState state, ObjectSyntax obj)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var creator = new ObjectCreator(obj, state);
            var fields = new List<ObjectField>();

            foreach (var field in obj.Fields.Children)
            {
                fields.Add((EngineInstruction.Execute(state, field).Value as ObjectField)!);
            }

            creator.Fields.AddRange(fields);
            return creator;
        }
    }
}
