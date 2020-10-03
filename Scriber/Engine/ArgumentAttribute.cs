using Scriber.Autocomplete;
using System;

namespace Scriber.Engine
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ArgumentAttribute : Attribute
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Type[]? Overrides { get; set; }
        public Type? ProposalProvider { get; set; }

        public IProposalProvider? GetProposalProvider()
        {
            if (ProposalProvider == null)
            {
                return null;
            }

            if (!ProposalProvider.IsSubclassOf(typeof(IProposalProvider)))
            {
                return null;
            }

            var provider = Activator.CreateInstance(ProposalProvider) as IProposalProvider;
            return provider;
        }
    }
}
