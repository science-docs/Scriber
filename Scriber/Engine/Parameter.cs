using Namotion.Reflection;
using Scriber.Autocomplete;
using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Scriber.Engine
{
    public class Parameter
    {
        public string Name { get; }
        public string? Description { get; }
        public IReadOnlyCollection<Type> Overrides { get; }
        public IProposalProvider? ProposalProvider { get; }
        public ContextualParameterInfo Info { get; }
        public Type Type => Info.OriginalType;
        public int Index => Info.ParameterInfo.Position;
        public bool IsNullable { get; }
        public bool IsOptional => Info.ParameterInfo.IsOptional;

        public Parameter(ParameterInfo info)
        {
            Info = info.ToContextualParameter();
            IsNullable = Info.IsNullable();
            var attribute = info.GetCustomAttribute<ArgumentAttribute>();
            Name = attribute?.Name ?? Info.Name;
            ProposalProvider = attribute?.GetProposalProvider();
            Overrides = attribute?.Overrides ?? Array.Empty<Type>();

            if (ProposalProvider == null && Type.IsEnum)
            {
                ProposalProvider = EnumProposalProvider.From(Type);
            }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool removeArgument)
        {
            return Type.FormattedName(Info, removeArgument) + " " + Name;
        }
    }
}
