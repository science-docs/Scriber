using Scriber.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Language
{
    public class ParserContext
    {
        public Resource? Resource { get; set; }
        public ParserIssueCollection Issues { get; } = new ParserIssueCollection();
        public TokenQueue Tokens { get; set; } = new TokenQueue(Array.Empty<Token>());

        private List<(SyntaxNode node, IEnumerable<TokenType>)> stopTokens = new List<(SyntaxNode node, IEnumerable<TokenType>)>();
    }
}
