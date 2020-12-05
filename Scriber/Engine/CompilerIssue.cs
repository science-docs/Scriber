using Scriber.Language;
using Scriber.Language.Syntax;
using System;

namespace Scriber.Engine
{
    public enum CompilerIssueType
    {
        Information = 1,
        Warning = 2,
        Error = 3
    }

    public class CompilerIssue
    {
        public TextSpan Span => Origin.Span;
        public string Message { get; set; }
        public SyntaxNode Origin { get; set; }
        public Exception? InnerException { get; set; }
        public CompilerIssueType Type { get; set; }

        public CompilerIssue(SyntaxNode origin, CompilerIssueType type, string message, Exception? innerException)
        {
            Type = type;
            Message = message;
            Origin = origin;
            InnerException = innerException;
        }
    }
}
