using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using Scriber.LSP.Handler;

namespace Scriber.LSP
{
    class Program
    {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
        private static void Main(string[] args) => MainAsync(args).Wait();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

        private static async Task MainAsync(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Stream? wsInOutput = null;
            Socket? accepted = null;
            var lspArg = args.FirstOrDefault(e => e.StartsWith("--lsp="));
            if (lspArg != null)
            {
                var port = lspArg["--lsp=".Length..];
                if (int.TryParse(port, out var portNumber))
                {
                    var ws = new Socket(SocketType.Stream, ProtocolType.IP);
                    ws.Bind(new IPEndPoint(IPAddress.Any, portNumber));
                    ws.Listen(128);
                    accepted = await ws.AcceptAsync();
                    wsInOutput = new NetworkStream(accepted, true);
                }
                else
                {
                    throw new Exception("Port: " + port + " is not a number.");
                }
            }


            var server = await LanguageServer.From(options =>
                options
                    .WithInput(wsInOutput ?? Console.OpenStandardInput())
                    .WithOutput(wsInOutput ?? Console.OpenStandardOutput())
                    .ConfigureLogging(
                        x => x
                            .AddLanguageProtocolLogging()
                            .SetMinimumLevel(LogLevel.Debug)
                    )
                    .WithHandler<TextDocumentHandler>()
                    .WithHandler<CompletionHandler>()
                    .WithServices(x => x.AddLogging(b => b.SetMinimumLevel(LogLevel.Trace)))
                    .WithServices(
                        services =>
                        {
                            services.AddSingleton<Editor.Environment>();
                        }
                    )
                    .OnInitialize((server, request, token) =>
                        {
                            if (request.RootPath != null)
                            {
                                Directory.SetCurrentDirectory(request.RootPath);
                            }
                            return Task.CompletedTask;
                        }
                    )
            );

            await server.WaitForExit;
        }
    }
}
