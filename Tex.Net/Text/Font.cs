using PdfSharpCore.Fonts;
using SixLabors.Fonts;
using System.IO;

namespace Tex.Net.Text
{
    public class Font
    {
        private static readonly FontCollection fonts = new FontCollection();

        private static readonly FontFamily serif = fonts.Install(LoadFont("cmu.serif-roman"));
        private static readonly FontFamily serifItalic = fonts.Install(LoadFont("cmu.serif-italic"));
        private static readonly FontFamily serifBold = fonts.Install(LoadFont("cmu.serif-bold"));
        private static readonly FontFamily serifBoldItalic = fonts.Install(LoadFont("cmu.serif-bolditalic"));

        private const float Dpi = 72;

        public static Font Serif { get; } = new Font(serif, 12);
        public static Font SerifItalic { get; } = new Font(serifItalic, 12);
        public static Font SerifBold { get; } = new Font(serifBold, 12);
        public static Font SerifBoldItalic { get; } = new Font(serifBoldItalic, 12);

        static Font()
        {
            GlobalFontSettings.FontResolver = new FontResolver();
        }

        internal SixLabors.Fonts.Font font { get; set; }

        public string Name { get; }
        public double Size { get; }

        private Font(FontFamily font, double size)
        {
            Name = font.Name;
            Size = size;
            this.font = font.CreateFont((float)size);
        }

        public double GetWidth(string text)
        {
            return TextMeasurer.Measure(text, new RendererOptions(font, Dpi)).Width;
        }

        private static Stream LoadFont(string name)
        {
            var asm = typeof(Font).Assembly;
            var fullName = $"Tex.Net.Resources.Fonts.{name}.ttf";
            using var stream = asm.GetManifestResourceStream(fullName);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            return new MemoryStream(ms.ToArray());
        }
    }
}
