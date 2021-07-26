using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Commands
{
    public enum LabelType
    {
        Section,
        Figure,
        Table
    }

    [Package]
    public static class Sections
    {
        public static DocumentLocal<int> Header1 = new(0);
        public static DocumentLocal<int> Header2 = new(0);
        public static DocumentLocal<int> Header3 = new(0);
        public static DocumentLocal<int> Header4 = new(0);
        public static DocumentLocal<int> Header5 = new(0);

        private static readonly DocumentLocal<int>[] Headers =
        {
            Header1, Header2, Header3, Header4, Header5
        };

        private const int MaxLevel = 5;

        [Command("Label")]
        public static void Label(CompilerState state, string name)
        {
            Label(state, LabelType.Section, name);
        }

        [Command("Label")]
        public static void Label(CompilerState state, LabelType type, string name)
        {
            List<TableElement> elementList = type switch
            {
                LabelType.Figure => state.Document.Variable(TableVariables.TableOfFigures),
                LabelType.Table => state.Document.Variable(TableVariables.TableOfTables),
                _ => state.Document.Variable(TableVariables.TableOfContent)
            } ?? throw new InvalidOperationException();
            var labels = LabelVariables.Labels.Get(state.Document) ?? throw new InvalidOperationException();
            labels[name] = elementList[^1];
        }

        [Command("Reference")]
        public static ITextLeaf? Reference(CompilerState state, Argument<string> label)
        {
            var labels = LabelVariables.Labels.Get(state.Document) ?? throw new InvalidOperationException();
            if (labels.TryGetValue(label.Value, out var tableElement))
            {
                if (string.IsNullOrWhiteSpace(tableElement.Preamble))
                {
                    state.Issues.Add(label.Source, CompilerIssueType.Warning, $"Element '{label.Value}' does not allow referencing");
                    return null;
                }

                return new ReferenceLeaf(tableElement.Reference.ReferencedElement, tableElement.Preamble);
            }
            else
            {
                state.Issues.Add(label.Source, CompilerIssueType.Warning, $"No label '{label.Value}' exists.");
                return null;
            }
        }

        [Command("TableOfContent")]
        public static LazyElement TableOfContent(CompilerState state)
        {
            return new LazyElement(() =>
            {
                var entries = state.Document.Variable(TableVariables.TableOfContent)!;

                var tableOfContent = new StackPanel { Orientation = Orientation.Vertical };
                tableOfContent.Elements.AddRange(entries);

                return tableOfContent;
            });
        }

        [Command("Heading")]
        public static Paragraph Heading(CompilerState state, Argument<int> level, Argument<Paragraph> content)
        {
            var pretext = AddTableOfContentInternal(state, true, level, content);
            return CreateHeading(state, level, content, pretext);
        }

        [Command("Heading*")]
        public static Paragraph HeadingStar(CompilerState state, Argument<int> level, Argument<Paragraph> content)
        {
            return CreateHeading(state, level, content);
        }

        [Command("AddTableOfContent")]
        public static Paragraph AddTableOfContent(CompilerState state, Argument<int> level, Argument<Paragraph> content)
        {
            AddTableOfContentInternal(state, false, level, content);
            content.Value.IsVisible = false;
            return content.Value;
        }

        private static string AddTableOfContentInternal(CompilerState state, bool withNumbering, Argument<int> level, Argument<Paragraph> content)
        {
            var lvl = level.Value;
            string preamble = string.Empty;
            if (withNumbering)
            {
                ResetNumbering(state.Document, lvl);
                preamble = CreatePretext(state.Document, lvl);
            }
            var paragraph = content.Value;
            var entry = new TableElement(preamble, lvl, paragraph.Clone(), paragraph);
            var toc = state.Document.Variable(TableVariables.TableOfContent) ?? throw new InvalidOperationException();
            toc.Add(entry);
            return preamble;
        }

        private static Paragraph CreateHeading(CompilerState state, Argument<int> level, Argument<Paragraph> content, string? preamble = null)
        {
            var lvl = level.Value;
            if (lvl < 1 || lvl > MaxLevel)
            {
                state.Issues.Add(level.Source, CompilerIssueType.Warning, "");
                lvl = Math.Clamp(lvl, 1, MaxLevel);
            }
            var paragraph = content.Value;
            paragraph.Tag = "h" + lvl;
            //paragraph.FontSize = lvl switch
            //{
            //    1 => 18,
            //    2 => 16,
            //    3 => 14,
            //    _ => 12
            //};
            //paragraph.Margin = new Thickness(14 - lvl * 2, 0);
            AddOutline(state, lvl, paragraph);

            if (preamble != null)
            {
                var text = new TextLeaf
                {
                    Content = preamble + " "
                };
                paragraph.Leaves.Insert(0, text);
            }

            return paragraph;
        }

        [Command("Section")]
        public static Paragraph Section(CompilerState state, Argument<Paragraph> content)
        {
            return Heading(state, new Argument<int>(content.Source, 1), content);
        }

        [Command("Section*")]
        public static Paragraph SectionStar(CompilerState state, Argument<Paragraph> content)
        {
            return HeadingStar(state, new Argument<int>(content.Source, 1), content);
        }

        [Command("Subsection")]
        public static Paragraph Subsection(CompilerState state, Argument<Paragraph> content)
        {
            return Heading(state, new Argument<int>(content.Source, 2), content);
        }

        [Command("Subsection*")]
        public static Paragraph SubsectionStar(CompilerState state, Argument<Paragraph> content)
        {
            return HeadingStar(state, new Argument<int>(content.Source, 2), content);
        }

        private static string CreatePretext(Document document, int level)
        {
            StringBuilder sb = new();
            var header = Headers[level - 1];

            var levelLocal = document.Variable(header);
            header.Set(document, levelLocal + 1);

            for (int i = 0; i < level; i++)
            {
                sb.Append(document.Variable(Headers[i]));
                if (i < level - 1)
                {
                    sb.Append(".");
                }
            }

            return sb.ToString();
        }

        private static void ResetNumbering(Document document, int startLevel)
        {
            for (int i = startLevel + 1; i < MaxLevel; i++)
            {
                Headers[i - 1].Set(document, 0);
            }
        }

        private static void AddOutline(CompilerState state, int level, Paragraph content)
        {
            if (state.Converters.TryConvert<string>(content, out var stringContent))
            {
                var doc = state.Document;
                var outlineChildren = GetParentOutline(doc.Outlines, 1, level)?.Children ?? doc.Outlines;
                outlineChildren.Add(new Outline(stringContent, content));
            }
        }

        private static Outline? GetParentOutline(List<Outline> outlines, int outlineLevel, int level)
        {
            if (outlineLevel >= level || outlines.Count == 0)
            {
                return null;
            }

            var last = outlines[^1];
            var child = GetParentOutline(last.Children, outlineLevel + 1, level);
            return child ?? last;
        }
    }
}
