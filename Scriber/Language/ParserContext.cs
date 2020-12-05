using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Language
{
    public class ParserContext
    {
        public Resource? Resource { get; set; }
        public ParserIssueCollection Issues { get; } = new ParserIssueCollection();
        public TokenQueue Tokens { get; set; } = new TokenQueue();
    }
}
