using Scriber.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Autocomplete
{
    public class EnumProposalProvider : IProposalProvider
    {
        public Type EnumType;
        public string[] Names;

        public EnumProposalProvider(Type enumType)
        {
            EnumType = enumType;
            Names = Enum.GetNames(EnumType);
        }

        public IEnumerable<Proposal> Propose(CompilerState state)
        {
            return Names.Select(e => new Proposal(e) { Type = ProposalType.EnumMember });
        }

        private static readonly Dictionary<Type, EnumProposalProvider> cache = new();

        public static EnumProposalProvider From(Type type)
        {
            if (!type.IsEnum)
            {
                throw new InvalidCastException("Type is not an enum");
            }

            lock (cache)
            {
                if (!cache.TryGetValue(type, out var provider))
                {
                    cache[type] = provider = new EnumProposalProvider(type);
                }

                return provider;
            }
        }
    }
}
