using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using Scriber.Editor;
using System.Threading;
using System.Threading.Tasks;

namespace Scriber.LSP.Handler
{
    public class TextDocumentHandler : TextDocumentSyncHandlerBase
    {
        public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri) => new TextDocumentAttributes(uri, "scriber");

        private readonly Environment environment;
        private readonly ILanguageServerFacade facade;

        public TextDocumentHandler(Environment env, ILanguageServerFacade facade)
        {
            environment = env;
            this.facade = facade;
        }

        public override Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
        {
            environment.Open(request.TextDocument.Uri.ToUri());
            return Unit.Task;
        }

        public override Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
        {
            var document = environment.GetDocument(request.TextDocument.Uri.ToUri());
            foreach (var change in request.ContentChanges)
            {
                if (change.Range == null)
                {
                    document.Change(new DocumentChange(change.Text));
                }
                else
                {
                    var start = document.GetOffset(change.Range.Start.Line, change.Range.Start.Character);
                    var end = document.GetOffset(change.Range.End.Line, change.Range.End.Character);
                    document.Change(new DocumentChange(change.Text, start, end));
                }
            }
            environment.Compile(request.TextDocument.Uri.ToUri());
            facade.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
            {
                Uri = request.TextDocument.Uri,
                Version = request.TextDocument.Version,
                Diagnostics = new Container<Diagnostic>(document.GenerateDiagnostics())
            });
            return Unit.Task;
        }

        public override Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        public override Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
        {
            environment.Close(request.TextDocument.Uri.ToUri());
            return Unit.Task;
        }

        protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentSyncRegistrationOptions(OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities.TextDocumentSyncKind.Incremental);
        }
    }
}
