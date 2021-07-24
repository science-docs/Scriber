using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            //IObserver<WorkDoneProgressReport> workDone = null!;

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


            var server = await LanguageServer.From(
                options =>
                    options
                       .WithInput(wsInOutput ?? Console.OpenStandardInput())
                       .WithOutput(wsInOutput ?? Console.OpenStandardOutput())
                       .ConfigureLogging(
                            x => x
                                .AddLanguageProtocolLogging()
                                .SetMinimumLevel(LogLevel.Debug)
                        )
                       .WithHandler<TextDocumentHandler>()
                       //.WithHandler<DidChangeWatchedFilesHandler>()
                       //.WithHandler<FoldingRangeHandler>()
                       //.WithHandler<MyWorkspaceSymbolsHandler>()
                       //.WithHandler<MyDocumentSymbolHandler>()
                       //.WithHandler<SemanticTokensHandler>()
                       .WithServices(x => x.AddLogging(b => b.SetMinimumLevel(LogLevel.Trace)))
                       .WithServices(
                            services => {
                                services.AddSingleton<Editor.Environment>();
                            }
                        )
                       .OnInitialize(
                            (server, request, token) => {
                                Directory.SetCurrentDirectory(request.RootPath);
                                return Task.CompletedTask;
                                //var manager = server.WorkDoneManager.For(
                                //    request, new WorkDoneProgressBegin
                                //    {
                                //        Title = "Server is starting...",
                                //        Percentage = 10,
                                //    }
                                //);
                                //workDone = manager;

                                //await Task.Delay(2000);

                                //manager.OnNext(
                                //    new WorkDoneProgressReport
                                //    {
                                //        Percentage = 20,
                                //        Message = "loading in progress"
                                //    }
                                //);
                            }
                        )
                       //.OnInitialized(
                       //     async (server, request, response, token) => {
                       //         workDone.OnNext(
                       //             new WorkDoneProgressReport
                       //             {
                       //                 Percentage = 40,
                       //                 Message = "loading almost done",
                       //             }
                       //         );

                       //         await Task.Delay(2000);

                       //         workDone.OnNext(
                       //             new WorkDoneProgressReport
                       //             {
                       //                 Message = "loading done",
                       //                 Percentage = 100,
                       //             }
                       //         );
                       //         workDone.OnCompleted();
                       //     }
                       // )
                       //.OnStarted(
                       //     async (languageServer, token) => {
                       //         using var manager = await languageServer.WorkDoneManager.Create(new WorkDoneProgressBegin { Title = "Doing some work..." });

                       //         manager.OnNext(new WorkDoneProgressReport { Message = "doing things..." });
                       //         await Task.Delay(10000);
                       //         manager.OnNext(new WorkDoneProgressReport { Message = "doing things... 1234" });
                       //         await Task.Delay(10000);
                       //         manager.OnNext(new WorkDoneProgressReport { Message = "doing things... 56789" });

                       //         var logger = languageServer.Services.GetService<ILogger<Foo>>();
                       //         var configuration = await languageServer.Configuration.GetConfiguration(
                       //             new ConfigurationItem
                       //             {
                       //                 Section = "typescript",
                       //             }, new ConfigurationItem
                       //             {
                       //                 Section = "terminal",
                       //             }
                       //         );

                       //         var baseConfig = new JObject();
                       //         foreach (var config in languageServer.Configuration.AsEnumerable())
                       //         {
                       //             baseConfig.Add(config.Key, config.Value);
                       //         }

                       //         logger.LogInformation("Base Config: {Config}", baseConfig);

                       //         var scopedConfig = new JObject();
                       //         foreach (var config in configuration.AsEnumerable())
                       //         {
                       //             scopedConfig.Add(config.Key, config.Value);
                       //         }

                       //         logger.LogInformation("Scoped Config: {Config}", scopedConfig);
                       //     }
                       // )
            );

            await server.WaitForExit;
        }
    }
}
