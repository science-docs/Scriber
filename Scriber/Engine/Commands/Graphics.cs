using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Util;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Graphics
    {
        [Command("Caption")]
        public static Paragraph Caption(Paragraph paragraph)
        {
            paragraph.Tag = "caption";
            paragraph.Margin = new Thickness(16, 0, 0, 0);
            return paragraph;
        }

        [Command("IncludeGraphics")]
        public static AbstractElement IncludeGraphics(CompilerState state, string imagePath, IncludeImageOptions? options = null)
        {
            // TODO: fetch online files if path starts with "http"
            var imagePathInfo = state.FileSystem.TryFindFile(imagePath, "png", "jpg", "gif", "bmp");
            
            if (imagePathInfo != null && (options == null || !options.Draft))
            {
                var bytes = state.FileSystem.File.ReadAllBytes(imagePathInfo.FullName);
                var image = new ImageElement(bytes, imagePath);

                if (options != null)
                {
                    image.Angle = options.Angle;
                    image.Scale = options.Scale;
                    image.Height = options.Height;
                    image.Width = options.Width;
                }

                return image;
            }
            else
            {
                return Paragraph.FromText(imagePath);
            }
        }

        public class IncludeImageOptions
        {
            public double Angle { get; set; }
            public bool Draft { get; set; }
            public double Scale { get; set; }
            public Unit Height { get; set; }
            public Unit Width { get; set; }
        }
    }
}
