using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Scriber.Editor;
using Scriber.Language;
using System.Collections.Generic;
using System.Text;

namespace Scriber.LSP
{
    public static class Utils
    {
        public static Container<Diagnostic> GenerateDiagnostics(this EditorDocument document)
        {
            var diagnostics = new List<Diagnostic>();
            var issues = document.Issues;
            foreach (var issue in issues)
            {
                diagnostics.Add(new Diagnostic
                {
                    Message = issue.Content,
                    Severity = issue.Severity.GetSeverity(),
                    Range = issue.Span.GetRange(document),
                    Source = "scriber",
                    Code = issue.Code
                });
            }

            return new Container<Diagnostic>(diagnostics);
        }

        private static DiagnosticSeverity GetSeverity(this IssueSeverity severity)
        {
            return severity switch
            {
                IssueSeverity.Error => DiagnosticSeverity.Error,
                IssueSeverity.Warning => DiagnosticSeverity.Warning,
                IssueSeverity.Information => DiagnosticSeverity.Information,
                _ => 0
            };
        }

        private static Range GetRange(this TextSpan span, EditorDocument document)
        {
            var (startLine, startChar) = document.GetPosition(span.Start);
            var (endLine, endChar) = document.GetPosition(span.End);
            return new Range(startLine, startChar, endLine, endChar);
        }
    }
}
