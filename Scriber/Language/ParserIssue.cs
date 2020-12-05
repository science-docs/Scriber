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
        public TextSpan Span { get; set; }
        public string Message { get; set; }
        public Exception? InnerException { get; set; }
        public ParserIssueType Type { get; set; }

        public ParserIssue(TextSpan span, ParserIssueType type, string message, Exception? innerException)
        {
            Span = span;
            Message = message;
            Type = type;
            InnerException = innerException;
        }
    }
}
