using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

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
            return new ParserResult(new Element[0], new ParserIssue[0]);
        }
    }
}
