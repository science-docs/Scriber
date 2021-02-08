using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Scriber.Text
{
    public class Font
    {
        public static Font Default { get; } = new Font("Times New Roman");

        private static readonly Dictionary<(string, FontWeight, double), XFont> fonts = new Dictionary<(string, FontWeight, double), XFont>();
        private static readonly XGraphics graphics = XGraphics.CreateMeasureContext(new XSize(100000, 100000), XGraphicsUnit.Point, XPageDirection.Downwards);

        //internal FontFamily FontFamily { get; set; }

        public string Name { get; }

        public Font(string name)
        {
            Name = name;
        }

        private readonly ConcurrentDictionary<(string, FontWeight), double> fontWidths = new ConcurrentDictionary<(string, FontWeight), double>();

        public double GetWidth(string text, double size, FontWeight weight)
        {
            if (fontWidths.ContainsKey((text, weight)))
            {
                return fontWidths[(text, weight)] * size;
            }
            else
            {
                var width = graphics.MeasureString(text, GetXFont(size, weight)).Width;
                fontWidths[(text, weight)] = width / size;
                return width;
            }
        }

        public XFont GetXFont(double size, FontWeight weight)
        {
            if (fonts.TryGetValue((Name, weight, size), out var font))
            {
                return font;
            }
            else
            {
                font = new XFont(Name, size, (XFontStyle)(int)weight);
                lock (fonts)
                {
                    fonts[(Name, weight, size)] = font;
                    return font;
                }
            }
        }

        //private static Stream LoadFont(string name)
        //{
        //    var asm = typeof(Font).Assembly;
        //    var fullName = $"Scriber.Resources.Fonts.{name}.ttf";
        //    using var stream = asm.GetManifestResourceStream(fullName);

        //    if (stream == null)
        //    {
        //        return new PdfSharpCore.Utils.FontResolver().GetFont(name);
        //    }

        //    using var ms = new MemoryStream();
        //    stream.CopyTo(ms);
        //    ms.Position = 0;
        //    return new MemoryStream(ms.ToArray());
        //}
    }
}
