using Scriber.Engine;
using Scriber.Language;
using System;

namespace Scriber.Editor
{
    public enum IssueSeverity
    {
        Error,
        Warning,
        Information
    }

    public class Issue
    {
        public TextSpan Span { get; set; }
        public string Content { get; set; }
        public IssueSeverity Severity { get; set; }
        public string Code { get; set; }

        public Issue(IssueSeverity severity, string content, string code, TextSpan span)
        {
            Severity = severity;
            Content = content;
            Code = code;
            Span = span;
        }

        public static Issue FromParserIssue(ParserIssue issue)
        {
            var severity = issue.Type switch
            {
                ParserIssueType.Information => IssueSeverity.Information,
                ParserIssueType.Warning => IssueSeverity.Warning,
                ParserIssueType.Error => IssueSeverity.Error,
                ParserIssueType.Log => throw new NotSupportedException(),
                _ => throw new NotImplementedException()
            };
            return new Issue(severity, issue.Message, "parser", issue.Span);
        }

        public static Issue FromCompilerIssue(CompilerIssue issue)
        {
            var severity = issue.Type switch
            {
                CompilerIssueType.Information => IssueSeverity.Information,
                CompilerIssueType.Warning => IssueSeverity.Warning,
                CompilerIssueType.Error => IssueSeverity.Error,
                _ => throw new NotImplementedException()
            };
            return new Issue(severity, issue.Message, "compiler", issue.Origin.Span);
        }
    }
}
