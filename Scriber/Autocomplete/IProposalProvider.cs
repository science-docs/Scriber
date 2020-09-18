using Scriber.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Autocomplete
{
    public interface IProposalProvider
    {
        public IEnumerable<Proposal> Propose(CompilerState state);
    }
}
