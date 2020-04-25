using System.Diagnostics;
using System.IO;
using System.Text;
using Tex.Net.Engine;
using Tex.Net.Language;

namespace Tex.Net.CLI
{
    class Program
    {
        static void Main()
        {
            Stopwatch watch = Stopwatch.StartNew();

            ReflectionLoader.Discover();

            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("\\setlength[baselinestretch]{2}");
            //sb.AppendLine("\\cfooter{Page \\thepage}");

            sb.AppendLine("\\begin{figure}");
            sb.AppendLine("\\includegraphics{test-image.jpg}");
            sb.AppendLine("\\end{figure}");

            sb.AppendLine("\\section{This is a section}");
            sb.AppendLine("After the \\red{first} section [follows] a pagebreak at page.");
            sb.AppendLine("This is the same paragraph");

            sb.AppendLine();
            sb.AppendLine("1 1 1 1 1 1 1 1 1 1");

            sb.AppendLine();
            sb.AppendLine();

            for (int i = 0; i < 100; i++)
            {
                sb.Append("1 ");
            }
            sb.AppendLine();
            sb.AppendLine("\\pagebreak");

            for (int i = 0; i < 98; i++)
            {
                sb.AppendLine("\\section{This is a section}");
                sb.AppendLine();
            }

            var tokens = Lexer.Tokenize(sb.ToString());
            var elements = Parser.Parse(tokens);
            var result = Compiler.Compile(elements);

            var document = result.Document;
            document.Measure();
            document.Arrange();
            document.Interlude();
            document.Measure();
            document.Arrange();

           var bytes = document.ToPdf();



            File.WriteAllBytes("test.pdf", bytes);

            watch.Stop();
            Debug.WriteLine($"Created document in {watch.ElapsedMilliseconds}ms");
        }
    }
}
