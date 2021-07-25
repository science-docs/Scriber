using Scriber.Language.Syntax;
using System;
using System.Collections.Generic;

namespace Scriber.Language
{
    public class ParserResult
    {
        public IEnumerable<SyntaxNode> Nodes { get; }
        public IEnumerable<ParserIssue> Issues { get; }

        public ParserResult(IEnumerable<SyntaxNode> nodes, IEnumerable<ParserIssue> issues)
        {
            Nodes = nodes;
            Issues = issues;
        }

        public SyntaxNode? NodeAt(int offset)
        {
            foreach (var node in Nodes)
            {
                var indexNode = node.ChildAtIndex(offset);
                if (indexNode != null)
                    return indexNode;
            }
            return null;
        }

        public static ParserResult Empty()
        {
            return new ParserResult(Array.Empty<SyntaxNode>(), Array.Empty<ParserIssue>());
        }
    }
}
