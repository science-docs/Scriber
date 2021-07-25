using Scriber.Editor;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Scriber.Autocomplete;
using System.Linq;

namespace Scriber.LSP.Handler
{
    public class CompletionHandler : ICompletionHandler
    {
        private readonly Environment environment;

        public CompletionHandler(Environment env)
        {
            environment = env;
        }

        public CompletionRegistrationOptions GetRegistrationOptions(CompletionCapability capability, ClientCapabilities clientCapabilities)
        {
            return new CompletionRegistrationOptions
            {
                TriggerCharacters = new("@"),
                AllCommitCharacters = new("(", ")", ";")
            };
        }

        public Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
        {
            var document = environment.GetDocument(request.TextDocument.Uri.ToUri());
            var offset = document.GetOffset(request.Position.Line, request.Position.Character);
            var proposals = environment.Completion.Get(document, offset);
            return Task.FromResult(new CompletionList(proposals.Select(ProposalToCompletion), false));
        }

        private CompletionItem ProposalToCompletion(Proposal proposal)
        {
            var item = new CompletionItem
            {
                Label = proposal.Content,
                Detail = proposal.Info,
                Kind = (CompletionItemKind)(int)proposal.Type,
                Documentation = proposal.Documentation == null ? null : new StringOrMarkupContent(new MarkupContent
                {
                    Kind = MarkupKind.Markdown,
                    Value = proposal.Documentation
                })
            };
            return item;
        }
    }
}
