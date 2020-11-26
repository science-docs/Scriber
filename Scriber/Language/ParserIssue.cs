using Scriber.Language.Syntax;
using System;

namespace Scriber.Language
{
    public enum ParserIssueType
    {
        Log = 0,
        Information = 1,
        Warning = 2,
        Error = 3
    }

    public class ParserIssue
    {
        public TextSpan Span => Origin.Span;
        public string Message { get; set; }
        public SyntaxNode Origin { get; set; }
        public Exception? InnerException { get; set; }
        public ParserIssueType Type { get; set; }

        public ParserIssue(SyntaxNode origin, ParserIssueType type, string message, Exception? innerException)
        {
            Type = type;
            Message = message;
            Origin = origin;
            InnerException = innerException;
        }

        public ParserIssue(Token origin, ParserIssueType type, string message, Exception? innerException)
        {
            Type = type;
            Message = message;
            Origin = new TextSyntax
            {
                Span = origin.Span
            };
            InnerException = innerException;
        }
    }
}
