﻿using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using Scriber.Editor;
using System.Threading;
using System.Threading.Tasks;

namespace Scriber.LSP.Handler
{
    public class TextDocumentHandler : TextDocumentSyncHandlerBase
    {
        public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri) => new(uri, "scriber");

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
            var document = environment.GetDocument(request.TextDocument.Uri.ToUri());
            environment.Compile(request.TextDocument.Uri.ToUri(), cancellationToken);
            PublishDiagnostics(request.TextDocument, document, cancellationToken);
            return Unit.Task;
        }

        public override Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
        {
            var document = environment.GetDocument(request.TextDocument.Uri.ToUri());
            foreach (var change in request.ContentChanges)
            {
                if (change.Range == null)
                {
                    document.Change(new(change.Text));
                }
                else
                {
                    var start = document.GetOffset(change.Range.Start.Line, change.Range.Start.Character);
                    var end = document.GetOffset(change.Range.End.Line, change.Range.End.Character);
                    document.Change(new(change.Text, start, end));
                }
            }
            environment.Compile(request.TextDocument.Uri.ToUri(), cancellationToken);
            PublishDiagnostics(request.TextDocument, document, cancellationToken);
            return Unit.Task;
        }

        private void PublishDiagnostics(TextDocumentIdentifier docIdentifier, EditorDocument document, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                facade.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
                {
                    Uri = docIdentifier.Uri,
                    Version = docIdentifier is OptionalVersionedTextDocumentIdentifier versioned ? versioned.Version : 0,
                    Diagnostics = new(document.GenerateDiagnostics())
                });
            }
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
            return new(TextDocumentSyncKind.Incremental);
        }
    }
}