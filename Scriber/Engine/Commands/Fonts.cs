using System.Linq;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Fonts
    {
        [Command("FontFamily")]
        public static Paragraph Fontfamily(CompilerState state, string fontName, Paragraph paragraph)
        {
            return paragraph;
        }

        /// <summary>
        /// Some Comment
        /// </summary>
        /// <param name="state">State Argument</param>
        /// <param name="size">some size</param>
        [Command("FontSize")]
        public static void FontSize(CompilerState state, Unit size)
        {
            FontVariables.FontSize.Set(state.Document, size.Point);
        }

        [Command("FontSize")]
        public static DocumentElement[] FontSize(Unit size, Argument<DocumentElement>[] children)
        {
            foreach (var child in children)
            {
                child.Value.FontSize = size.Point;
            }
            return children.Select(e => e.Value).ToArray();
        }

        [Command("FootnoteSize")]
        public static void FootnoteSize(CompilerState state, Unit size)
        {
            FontVariables.FootnoteSize.Set(state.Document, size.Point);
        }
    }
}
