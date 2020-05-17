using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Engine
{
    public enum CompilerIssueType
    {
        Information,
        Warning,
        Error
    }

    public class CompilerIssue
    {
        public int Index { get; set; }
        public string Message { get; set; }
        public Exception? InnerException { get; set; }
        public CompilerIssueType Type { get; set; }

        public CompilerIssue(CompilerIssueType type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
