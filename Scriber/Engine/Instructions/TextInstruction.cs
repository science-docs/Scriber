using Scriber.Language.Syntax;

namespace Scriber.Engine.Instructions
{
    public class TextInstruction : EngineInstruction<TextSyntax>
    {
        public override object Execute(CompilerState state, TextSyntax text)
        {
            return new Layout.Document.TextLeaf
            {
                Content = text.Text
            };
        }
    }
}
