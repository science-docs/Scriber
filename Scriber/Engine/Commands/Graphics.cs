using System.IO;
using Scriber.Layout.Document;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Graphics
    {
        [Command("caption")]
        public static Paragraph Caption(Paragraph paragraph)
        {
            paragraph.Tag = "caption";
            paragraph.Margin = new Layout.Thickness(16, 0, 0, 0);
            return paragraph;
        }

        [Command("includegraphics")]
        public static ImageElement IncludeGraphics(CompilerState state, string imagePath, DocumentVariable? variables = null)
        {
            var bytes = File.ReadAllBytes(imagePath);
            var image = new ImageElement(bytes, imagePath);
            return image;
        }
    }
}
