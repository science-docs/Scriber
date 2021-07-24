using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scriber.LSP.Handler
{
    public class ExecuteCommandHandler : IExecuteCommandHandler<string>
    {
        private readonly Editor.Environment environment;

        public ExecuteCommandHandler(Editor.Environment env)
        {
            environment = env;
        }

        public ExecuteCommandRegistrationOptions GetRegistrationOptions(ExecuteCommandCapability capability, ClientCapabilities clientCapabilities)
        {
            return new ExecuteCommandRegistrationOptions
            {
                Commands = new Container<string>("toHtml", "toPdf")
            };
        }

        public Task<string> Handle(ExecuteCommandParams<string> request, CancellationToken cancellationToken)
        {
            var array = request.Arguments;
            Uri? uri = null;
            if (array != null)
            {
                uri = new Uri(array.First.ToString(), UriKind.Absolute);
            }

            if (uri != null && request.Command == "toPdf")
            {
                var state = environment.Compile(uri);
                return Task.FromResult(Convert.ToBase64String(state.Document.ToPdf()));
            }

            return Task.FromResult("");
        }
    }
}
