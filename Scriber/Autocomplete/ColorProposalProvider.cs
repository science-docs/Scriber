using Scriber.Engine;
using Scriber.Text;
using Scriber.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Autocomplete
{
    public class ColorProposalProvider : IProposalProvider
    {
        public IEnumerable<Proposal> Propose(CompilerState state)
        {
            var knownColors = new List<string>(Enum.GetNames(typeof(KnownColor)));

            var customColors = state.Document.Variable(ColorVariables.CustomColors);
            knownColors.AddRange(customColors.Keys);
            knownColors.Sort();

            return knownColors.Select(e => new Proposal(e));
        }
    }
}
