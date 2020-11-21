using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using Scriber.Engine;
using Scriber.Language;
using Scriber.Logging;
using Scriber.Maths;

namespace Scriber.CLI
{
    class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Stopwatch watch = Stopwatch.StartNew();

            var context = new Context();
            context.Logger.Level = LogLevel.Debug;
            new ReflectionLoader().Discover(context, typeof(TestPackage).Assembly);

            StringBuilder sb = new StringBuilder();

            var math = MathParser.Evaluate("3.4");

            //sb.AppendLine("\\setlength[baselinestretch]{2}");
            //sb.AppendLine("@IncludePdf(tfl.pdf, { Fields: [ 4711, Lorem ipsum dolor sit amet consetetur sadipscing elitr sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, A16a ] })");
            //sb.AppendLine("Some text");
            sb.AppendLine("@CenterHeader(@IncludeGraphics(tfl.png))");
            sb.AppendLine("@CenterFooter(@ThePage)");
            sb.AppendLine("@FontSize(3*4)");
            sb.AppendLine("@Heading*(1, A)");
            sb.AppendLine("@TableOfContent()");
            sb.AppendLine("@Heading(1, B)");
            sb.AppendLine("Test@Footnote(\"\")");
            sb.AppendLine("@Pagebreak()").AppendLine();
            sb.AppendLine("@Color(Red, Red Text)").AppendLine();
            sb.AppendLine("@Pagebreak()");
            sb.AppendLine("@Pagebreak()");
            sb.AppendLine("@Pagebreak()");
            sb.AppendLine("@SetPageNumbering(@TextWidth() / 37, RomanUpper)");
            sb.AppendLine("@Pagebreak()");
            sb.AppendLine("@Pagebreak()");
            sb.AppendLine("@Pagebreak()");
            ////sb.AppendLine("@Include(include.sc)");
            ////sb.AppendLine("\\section{This is a section}");

            ////sb.AppendLine("\\begin{figure}");
            ////sb.AppendLine("\\centering");
            ////sb.AppendLine("\\includegraphics{test-image.png}");
            ////sb.AppendLine("\\caption{First Test Image}");
            ////sb.AppendLine("\\end{figure}");
            ////sb.AppendLine("@IncludePdf(https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf)").AppendLine();
            ////for (int i = 0; i < 1000; i++)
            ////{
            //sb.Append("Just some text. ");


            ////if (i % 100 == 50)
            ////{
            //sb.AppendLine("@Footnote(\"Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.\")");
            ////}
            ////}

            //sb.AppendLine().AppendLine("@VerticalSpace(590)");

            ////sb.Append("Some more text");

            ////sb.AppendLine("@test({ })");
            ////sb.AppendLine();

            ////sb.AppendLine("@BibliographyStyle(ieee.csl)");
            ////sb.AppendLine("@Bibliography(lib.bib)");

            //sb.AppendLine("@Section*(Table of Contents)");

            //sb.AppendLine("@TableOfContent()");

            ////////sb.AppendLine();
            ////////sb.AppendLine();
            ////////sb.AppendLine();
            //sb.AppendLine("@Section(\"This is a two line section: Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt est\")");
            //sb.AppendLine("@Subsection(This is a subsection)");
            //sb.AppendLine("@Header(3, This is a header)");
            //sb.AppendLine("@Header(3, Another header)");
            //sb.AppendLine("@Subsection(This is a subsection)");
            //sb.AppendLine("@Header(3, This is a header)");
            //sb.AppendLine("@Header(3, Another header)");
            //sb.AppendLine("@List(upperalpha) {");
            //sb.AppendLine("@ListItem(abc)");
            //sb.AppendLine("@ListItem(cde)");
            //sb.AppendLine("@ListItem(fgh)");
            //sb.AppendLine("} @Header(3, Another header)");
            //sb.AppendLine("@Header(3, Another header)");
            ////sb.AppendLine("@IncludePdf(dummy.pdf)");
            //sb.AppendLine("@Section(Second Section@Footnote(Title Footnote))");
            //sb.AppendLine("@Subsection(This is a subsection)");
            //sb.AppendLine("@IncludePdf(dummy.pdf)");
            //////sb.AppendLine();
            //////sb.AppendLine("Aft\\-er the @Color(blue, first) section [follows]@Footnote(Some Paragraph Content, \"1\") a @Bold(pagebreak).@Cite(Zanoni.2015)");
            //sb.AppendLine("The following is a citation: @Cite(\"Zanoni.2015\")");
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine("But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure? On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best, every pleasure is to be welcomed and every pain avoided. But in certain circumstances and owing to the claims of duty or the obligations of business it will frequently occur that ");
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine("\\begin{figure}");
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine("\\centering");
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine("\\includegraphics{test-image.png}");
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine("\\caption{Test Image}");
            //////sb.AppendLine();
            //////sb.AppendLine();
            //////sb.AppendLine("\\end{figure}");

            //////sb.AppendLine("\\begin{figure}");
            //////sb.AppendLine("\\centering");
            //////sb.AppendLine("\\includegraphics{test-image.png}");
            //////sb.AppendLine("\\caption{Second Test Image}");
            //////sb.AppendLine("\\end{figure}");

            //////sb.AppendLine("\\begin{figure}");
            //////sb.AppendLine("\\centering");
            //////sb.AppendLine("\\includegraphics{test-image.png}");
            //////sb.AppendLine("\\caption{Second Test Image}");
            //////sb.AppendLine("\\end{figure}");
            //sb.AppendLine("@UseTemplate(nak.sc)");
            //sb.AppendLine("@TableOfFigures()");
            //sb.AppendLine("@Acronyms({ CPU: Computer Processing Unit, FP: Floating Point })").AppendLine();
            //sb.AppendLine("This is the full name '@Acronym(FP)' for acronym '@Acronym(FP)'.");
            //sb.AppendLine("@VerticalSpace(550pt)");
            //sb.AppendLine("@FootnoteSize(6)");
            //sb.AppendLine("@FontSize(14)");
            //sb.AppendLine("Some text");

            //sb.AppendLine("@Section(First)");
            //sb.AppendLine("@Subsection(Second)");
            //sb.AppendLine("@Label(section, x)");
            //sb.AppendLine("blabla @Reference(x)");
            //sb.AppendLine("@Figure(@Centering(), @IncludeGraphics(https://asia.olympus-imaging.com/content/000107506.jpg), @Caption(Second Test Image@Footnote(More Footnotes)))");
            //sb.AppendLine("@FontSize(20, Some Larger Text, Some more text)").AppendLine();
            //sb.AppendLine("");
            //sb.AppendLine("");
            ////sb.AppendLine("@Caption(Second Test Image@Footnote(More Footnotes))");
            //sb.AppendLine("}");

            //for (int i = 0; i < 100; i++)
            //{
            //    sb.AppendLine().AppendLine("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.");
            //}



            //sb.AppendLine("@Section(More Sections)");

            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Pagebreak()");
            //sb.AppendLine("@Section(Bibliography)");
            //sb.AppendLine("@PrintBibliography()");

            ////sb.AppendLine();
            ////sb.AppendLine("1 1 1 1 1 1 1 1 1 1");

            ////sb.AppendLine();
            ////sb.AppendLine();

            ////for (int i = 0; i < 100; i++)
            ////{
            ////    sb.Append("1 ");
            ////}
            ////sb.AppendLine("\\begin{figure}");
            ////sb.AppendLine("\\centering");
            ////sb.AppendLine("\\includegraphics{test-image.png}");
            ////sb.AppendLine("\\caption{Third Test Image}");
            ////sb.AppendLine("\\end{figure}");

            ////sb.AppendLine();
            //////sb.AppendLine("\\pagebreak");

            ////for (int i = 0; i < 98; i++)
            ////{
            ////    sb.AppendLine("\\section{This is a section}");
            ////    sb.AppendLine();
            ////}

            ////sb.AppendLine("@figure()\n{ }");
            //sb.AppendLine("@test({ })");
            //sb.Append("@Figure() { }");
            //sb.AppendLine("@includegraphics(\"test-image.png\")");

            var tokens = Lexer.Tokenize(sb.ToString());
            context.Logger.Logged += Logger_Logged;
            var parserResult = Parser.Parse(tokens, null, context.Logger);
            var result = Compiler.Compile(context, parserResult.Elements);

            var document = result.Document;
            document.Run(context.Logger);

            var s = sb.ToString();
            var bytes = document.ToPdf();

            System.IO.File.WriteAllBytes("test.pdf", bytes);

            watch.Stop();
            Debug.WriteLine($"Created document in {watch.ElapsedMilliseconds}ms");
        }

        private static void Logger_Logged(Log log)
        {
            var content = string.Join("", log.GetFullMessageParts());
            Console.WriteLine(content);
            Debug.WriteLine(content);
        }
    }
}
