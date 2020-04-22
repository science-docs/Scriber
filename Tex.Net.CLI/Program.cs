using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Tex.Net.Engine;
using Tex.Net.Language;
using Tex.Net.Layout;
using Tex.Net.Layout.Document;

namespace Tex.Net.CLI
{
    class Program
    {
        static void Main()
        {
            Stopwatch watch = Stopwatch.StartNew();

            CommandCollection.Discover();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\\cfooter{Page \\thepage}");
            sb.AppendLine("\\section{This is a section}");
            sb.AppendLine("After the \\red{first} section follows a pagebreak at page.");
            sb.AppendLine("This is the same paragraph");

            sb.AppendLine();
            sb.AppendLine("1 1 1 1 1 1 1 1 1 1");

            sb.AppendLine();
            sb.AppendLine();

            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    sb.Append("\\thepage ");
                }
                sb.AppendLine();
                sb.AppendLine();
                
            }
            sb.AppendLine();
            sb.AppendLine("\\pagebreak");

            for (int i = 0; i < 98; i++)
            {
                sb.AppendLine("\\section{This is a section}");
                sb.AppendLine();
            }

            var tokens = Lexer.Tokenize(sb.ToString());
            var element = Parser.Parse(tokens);
            var result = Compiler.Compile(element);

            var document = result.Document;
            document.Measure();
            document.Arrange();
            document.Interlude();
            //document.Reflow();
            document.Measure();
            document.Arrange();

            var bytes = document.ToPdf();

            File.WriteAllBytes("test.pdf", bytes);

            watch.Stop();
            Debug.WriteLine($"Created document in {watch.ElapsedMilliseconds}ms");
        }
    }
}
