using Scriber.Language.Syntax;

namespace Scriber.Engine.Instructions
{
    public class QuoteInstruction : EngineInstruction<QuoteSyntax>
    {
        private bool open = true;

        public override object? Evaluate(CompilerState state, QuoteSyntax node)
        {
            var locale = state.Document.Locale;
            var result = open ? locale.OpenQuote : locale.CloseQuote;
            open = !open;
            return result;
        }
    }
}
