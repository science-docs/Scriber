using Scriber.Language.Syntax;
using System;

namespace Scriber.Engine.Instructions
{
    public class TextInstruction : EngineInstruction<TextSyntax>
    {
        public override object Evaluate(CompilerState state, TextSyntax text)
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state));
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            return new Layout.Document.TextLeaf
            {
                Content = text.Text,
                Source = text
            };
        }
    }
}
