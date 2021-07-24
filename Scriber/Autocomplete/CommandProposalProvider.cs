using Scriber.Engine;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Autocomplete
{
    public class CommandProposalProvider : IProposalProvider
    {
        public IEnumerable<Proposal> Propose(CompilerState state)
        {
            return state.Commands.GetNames().Select(e => new Proposal(e));
        }
    }
}
