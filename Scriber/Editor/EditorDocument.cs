using Scriber.Language;
using Scriber.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Editor
{
    public class EditorDocument
    {
        public Uri Uri { get; set; }
        public string Content
        {
            get => content;
            set
            {
                content = value;
                indices = CalculateIndices(content);
            }
        }
        public bool Open { get; set; }
        public List<Issue> Issues { get; } = new List<Issue>();

        public ParserResult ParserResult { get; private set; }

        private int[] indices = Array.Empty<int>();
        private string content = "";

        public EditorDocument(Uri uri, string content)
        {
            Uri = uri;
            Content = content;
            ParserResult = new ParserResult(Array.Empty<SyntaxNode>(), Array.Empty<ParserIssue>());
            BuildParserResult();
        }

        public (int line, int column) GetPosition(int index)
        {
            for (int i = 1; i < indices.Length; i++)
            {
                if (indices[i] >= index)
                {
                    return (i, index - indices[i - 1]);
                }
            }
            return (0, index);
        }

        public int GetOffset(int line, int column)
        {
            if (line > indices.Length)
            {
                return Content.Length;
            }

            int index = indices[line - 1];
            return index + column;
        }

        public void Change(DocumentChange change)
        {
            Issues.RemoveAll(e => e.Code == "parser");
            ApplyChange(change);
            BuildParserResult();
        }

        private void ApplyChange(DocumentChange change)
        {
            var length = change.Text.Length;
            var delta = change.Start - change.End + change.Text.Length;
            var start = Content[..change.Start];
            var end = Content[change.End..];
            Content = start + change.Text + end;
            UpdateIssueMarkers(change.Start, delta);
        }

        private void BuildParserResult()
        {
            ParserResult = Parser.ParseFromString(Content, new Resource(Uri, ""));
            Issues.AddRange(ParserResult.Issues.Select(e => Issue.FromParserIssue(e)));
        }

        private int[] CalculateIndices(ReadOnlySpan<char> content)
        {
            List<int> indices = new(this.indices.Length);

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '\n')
                {
                    indices.Add(i + 1);
                }
            }

            return indices.ToArray();
        }

        private void UpdateIssueMarkers(int offset, int delta)
        {
            foreach (var issue in Issues)
            {
                var span = issue.Span;
                var start = span.Start;
                var end = span.End;
                if (delta > 0)
                {
                    var changeEnd = offset + delta;
                    if (end > changeEnd)
                    {
                        end += delta;
                    }
                    if (start >= changeEnd)
                    {
                        start += delta;
                    }
                    else if (start >= offset)
                    {
                        start = changeEnd;
                    }
                }
                else if (delta < 0)
                {
                    var changeStart = offset + delta;

                    if (end > offset)
                    {
                        end += delta;
                    }
                    if (start >= offset)
                    {
                        start += delta;
                    }
                    else if (start >= changeStart)
                    {
                        start = offset;
                    }
                }
                issue.Span = new TextSpan(start, end - start, span.Line);
            }
        }
    }
}
