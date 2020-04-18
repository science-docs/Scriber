using System.IO;
using System.Linq;
using Tex.Net.Layout;
using Tex.Net.Layout.Document;
using Tex.Net.Text;

namespace Tex.Net.CLI
{
    class Program
    {
        static void Main()
        {
            var tokens = Language.Lexer.Tokenize("\\TeX{argument} is a {\\tt nice language}. % This is a \\comment\n\nAnd some other paragraph");
            var elements = Language.Parser.Parse(tokens).ToArray();

            var document = new Document();
            var page = new DocumentPage();
            document.Pages.Add(page);

            page.Size = new Size(595, 800);

            var paragraph = new Paragraph();
            paragraph.Add(new Layout.Document.Text
            {
                Font = Font.Serif,
                Content = "Lorem ipsum dolor sit amet.",
                Size = 12
            });
            paragraph.Measure(page.Size);
            var split = paragraph.Split();

            foreach (var items in split)
            {
                items.Arrange(new Rectangle(50, 50, 10000, 1000));
            }

            page.Blocks.AddRange(split);

            var bytes = document.ToPdf();

            File.WriteAllBytes("test.pdf", bytes);
        }
    }
}
