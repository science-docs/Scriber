using Scriber.Engine;
using Scriber.Variables;
using System.Collections.Generic;

namespace Scriber.Autocomplete
{
    public class AcronymProposalProvider : IProposalProvider
    {
        public IEnumerable<Proposal> Propose(CompilerState state)
        {
            var acronyms = AcronymVariables.Acronyms.Get(state.Document);

            foreach (var (name, full) in acronyms)
            {
                yield return new Proposal(name) { Info = full };
            }
        }
    }
}
