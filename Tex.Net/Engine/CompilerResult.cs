using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public class CompilerResult
    {
        public Document Document { get; }
        public bool Success => Document != null;
        public List<CompilerIssue> Issues { get; } = new List<CompilerIssue>();

        public CompilerResult(Document document)
        {
            Document = document;
        }
    }
}
