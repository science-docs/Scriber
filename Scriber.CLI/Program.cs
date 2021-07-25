using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Scriber.Engine;
using Scriber.Logging;

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

            var resource = context.ResourceManager.Get(new Uri("C:\\Users\\Mark\\Desktop\\Docdown\\main.sc"));
            context.Logger.Logged += Logger_Logged;
            var result = Compiler.Compile(context, resource, CancellationToken.None);

            var document = result.Document;

            document.Symbols.Count();
            document.Run(context.Logger);

            var bytes = document.ToPdf();

            System.IO.File.WriteAllBytes("test.pdf", bytes);

            watch.Stop();
            Console.WriteLine($"Created document in {watch.ElapsedMilliseconds}ms");
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
