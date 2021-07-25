using Scriber.Engine;
using System.Collections.Generic;

namespace Scriber.Autocomplete
{
    public class CitationProposalProvider : IProposalProvider
    {
        public IEnumerable<Proposal> Propose(CompilerState state)
        {
            var citations = state.Document.Citations;

            if (citations != null)
            {
                foreach (var citation in citations.GetCitations())
                {
                    yield return new Proposal(citation.Key) { Type = ProposalType.Field };
                }
            }
        }
    }
}
