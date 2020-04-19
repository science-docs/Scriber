using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tex.Net.Engine;
using Tex.Net.Language;
using Tex.Net.Layout;
using Tex.Net.Layout.Document;
using Tex.Net.Text;

namespace Tex.Net.CLI
{
    class Program
    {
        static void Main()
        {
            Stopwatch watch = Stopwatch.StartNew();

            CommandCollection.Discover();

            var tokens = Lexer.Tokenize("\\section{This is just some text}");//("\\TeX{a}{b} is a {\\tt nice language}. % This is a \\comment\n\nAnd some other paragraph");
            var elements = Parser.Parse(tokens).First();
            var result = Compiler.Compile(elements);

            var document = result.Document;
            var page = new DocumentPage();
            document.Pages.Add(page);

            page.Size = new Size(595, 800);

            var elms = document.Elements;
            var split = new List<Block>();
            foreach (var el in elms)
            {
                el.Measure(page.Size);
                split.AddRange(el.Split());
            }

            foreach (var items in split)
            {
                items.Arrange(new Rectangle(0, 50, 10000, 1000));
            }

            page.Blocks.AddRange(split);

            var bytes = document.ToPdf();

            File.WriteAllBytes("test.pdf", bytes);

            watch.Stop();
            Debug.WriteLine($"Created document in {watch.ElapsedMilliseconds}ms");
        }
    }
}
