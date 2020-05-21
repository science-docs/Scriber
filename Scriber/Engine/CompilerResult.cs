using System.Collections.Generic;
using System.Linq;

namespace Scriber.Engine
{
    public class CompilerResult
    {
        public Document Document { get; }
        public bool Success => !Issues.Any(e => e.Type == CompilerIssueType.Error);
        public List<CompilerIssue> Issues { get; } = new List<CompilerIssue>();

        public CompilerResult(Document document, IEnumerable<CompilerIssue> issues)
        {
            Document = document;
            Issues.AddRange(issues);
        }
    }
}
