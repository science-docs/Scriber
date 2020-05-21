using System.Diagnostics;
using System.IO;
using System.Text;
using Scriber.Engine;
using Scriber.Language;
using Scriber.Logging;

namespace Scriber.CLI
{
    class Program
    {
        static void Main()
        {
            Stopwatch watch = Stopwatch.StartNew();

            ReflectionLoader.Discover(typeof(TestPackage).Assembly);

            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("\\setlength[baselinestretch]{2}");
            //sb.AppendLine("\\cfooter{Page \\thepage}");

            //sb.AppendLine("\\section{This is a section}");

            //sb.AppendLine("\\begin{figure}");
            //sb.AppendLine("\\centering");
            //sb.AppendLine("\\includegraphics{test-image.png}");
            //sb.AppendLine("\\caption{First Test Image}");
            //sb.AppendLine("\\end{figure}");

            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine("@section(This is a section)");
            //sb.AppendLine();
            //sb.AppendLine("Aft\\-er the @color(beige, first) section [follows]@footnote(Some Paragraph Content) a pagebreak");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine("But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure? On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best, every pleasure is to be welcomed and every pain avoided. But in certain circumstances and owing to the claims of duty or the obligations of business it will frequently occur that ");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine("\\begin{figure}");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine("\\centering");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine("\\includegraphics{test-image.png}");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine("\\caption{Test Image}");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine("\\end{figure}");

            //sb.AppendLine("\\begin{figure}");
            //sb.AppendLine("\\centering");
            //sb.AppendLine("\\includegraphics{test-image.png}");
            //sb.AppendLine("\\caption{Second Test Image}");
            //sb.AppendLine("\\end{figure}");

            //sb.AppendLine("\\begin{figure}");
            //sb.AppendLine("\\centering");
            //sb.AppendLine("\\includegraphics{test-image.png}");
            //sb.AppendLine("\\caption{Second Test Image}");
            //sb.AppendLine("\\end{figure}");

            //sb.AppendLine("\\begin{figure}");
            //sb.AppendLine("\\centering");
            //sb.AppendLine("\\includegraphics{test-image.png}");
            //sb.AppendLine("\\caption{Second Test Image}");
            //sb.AppendLine("\\end{figure}");

            //sb.AppendLine();
            //sb.AppendLine("1 1 1 1 1 1 1 1 1 1");

            //sb.AppendLine();
            //sb.AppendLine();

            //for (int i = 0; i < 100; i++)
            //{
            //    sb.Append("1 ");
            //}
            //sb.AppendLine("\\begin{figure}");
            //sb.AppendLine("\\centering");
            //sb.AppendLine("\\includegraphics{test-image.png}");
            //sb.AppendLine("\\caption{Third Test Image}");
            //sb.AppendLine("\\end{figure}");

            //sb.AppendLine();
            ////sb.AppendLine("\\pagebreak");

            //for (int i = 0; i < 98; i++)
            //{
            //    sb.AppendLine("\\section{This is a section}");
            //    sb.AppendLine();
            //}
            sb.AppendLine("@test(TestValue {a: d, key: value, next: null})");
            //sb.AppendLine("@figure()\n{ }");
            //sb.AppendLine("@test(null)");
            //sb.Append("@Figure() { }");
            //sb.AppendLine("@includegraphics(\"test-image.png\")");

            var tokens = Lexer.Tokenize(sb.ToString());
            Logger logger = new Logger();
            logger.Logged += Logger_Logged;
            var elements = Parser.Parse(tokens);
            var result = Compiler.Compile(elements, logger);

            var document = result.Document;
            document.Run(logger);

            var bytes = document.ToPdf();



            //File.WriteAllBytes("test.pdf", bytes);

            watch.Stop();
            Debug.WriteLine($"Created document in {watch.ElapsedMilliseconds}ms");
        }

        private static void Logger_Logged(Log log)
        {
            Debug.WriteLine(string.Join("", log.GetFullMessageParts()));
        }
    }
}
