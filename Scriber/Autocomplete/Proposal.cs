using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Autocomplete
{
    public class Proposal
    {
        public string Content { get; set; }
        public string? Info { get; set; }
        public string? Documentation { get; set; }
        public ProposalType Type { get; set; } = ProposalType.Text;

        public Proposal(string content)
        {
            Content = content;
        }
    }

    public enum ProposalType
    {
        Text = 1,
        Method = 2,
        Function = 3,
        Constructor = 4,
        Field = 5,
        Variable = 6,
        Class = 7,
        Interface = 8,
        Module = 9,
        Property = 10,
        Unit = 11,
        Value = 12,
        Enum = 13,
        Keyword = 14,
        Snippet = 15,
        Color = 16,
        File = 17,
        Reference = 18,
        Folder = 19,
        EnumMember = 20,
        Constant = 21,
        Struct = 22,
        Event = 23,
        Operator = 24,
        TypeParameter = 25
    }
}
