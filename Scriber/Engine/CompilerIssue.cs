using Scriber.Language;
using System;

namespace Scriber.Engine
{
    public enum CompilerIssueType
    {
        Log = 0,
        Information = 1,
        Warning = 2,
        Error = 3
    }

    public class CompilerIssue
    {
        public int Index => Origin.Index;
        public int Length => Origin.Length;
        public int Line => Origin.Line;
        public string Message { get; set; }
        public Element Origin { get; set; }
        public Exception? InnerException { get; set; }
        public CompilerIssueType Type { get; set; }

        public CompilerIssue(Element origin, CompilerIssueType type, string message, Exception? innerException)
        {
            Type = type;
            Message = message;
            Origin = origin;
            InnerException = innerException;
        }
    }
}
