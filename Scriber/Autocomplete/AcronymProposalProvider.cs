using Scriber.Engine;
using Scriber.Engine.Converter;
using Scriber.Variables;
using System;
using System.Collections.Generic;

namespace Scriber.Autocomplete
{
    public class AcronymProposalProvider : IProposalProvider
    {
        public IEnumerable<Proposal> Propose(CompilerState state)
        {
            var acronyms = AcronymVariables.Acronyms.Get(state.Document) ?? throw new InvalidOperationException();

            foreach (var (name, full) in acronyms)
            {
                yield return new Proposal(name) { Info = ParagraphConverter.ToString(full), Type = ProposalType.Reference };
            }
        }
    }
}
