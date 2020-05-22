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
        public int Index => Origin.Index;
        public int Length => Origin.Length;
        public int Line => Origin.Line;
        public string Message { get; set; }
        public Element Origin { get; set; }
        public Exception? InnerException { get; set; }
        public ParserIssueType Type { get; set; }

        public ParserIssue(Element origin, ParserIssueType type, string message, Exception? innerException)
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
            Origin = new Element(null, ElementType.Text, origin.Index, origin.Line)
            {
                Length = origin.Length
            };
            InnerException = innerException;
        }
    }
}
