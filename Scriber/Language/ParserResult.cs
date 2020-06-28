using System;
using System.Collections.Generic;

namespace Scriber.Language
{
    public class ParserResult
    {
        public IEnumerable<Element> Elements { get; }
        public IEnumerable<ParserIssue> Issues { get; }

        public ParserResult(IEnumerable<Element> elements, IEnumerable<ParserIssue> issues)
        {
            Elements = elements;
            Issues = issues;
        }

        public static ParserResult Empty()
        {
            return new ParserResult(Array.Empty<Element>(), Array.Empty<ParserIssue>());
        }
    }
}
