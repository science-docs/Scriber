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

        public IEnumerable<Proposal> Propose(CommandCollection commands, string prefix = "")
        {
            return commands.GetNames().Where(e => e.StartsWith(prefix)).Select(e => new Proposal(e) { Type = ProposalType.Method });
        }
    }
}
