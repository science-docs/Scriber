using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Autocomplete
{
    public class Proposal
    {
        public string Content { get; set; }
        public string? Info { get; set; }

        public Proposal(string content)
        {
            Content = content;
        }
    }
}
