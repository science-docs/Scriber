using System;
using System.Text;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Sections
    {
        public static DocumentLocal<int> Header1 = new DocumentLocal<int>(0);
        public static DocumentLocal<int> Header2 = new DocumentLocal<int>(0);
        public static DocumentLocal<int> Header3 = new DocumentLocal<int>(0);
        public static DocumentLocal<int> Header4 = new DocumentLocal<int>(0);
        public static DocumentLocal<int> Header5 = new DocumentLocal<int>(0);

        private static readonly DocumentLocal<int>[] Headers =
        {
            Header1, Header2, Header3, Header4, Header5
        };

        private const int MaxLevel = 5;

        [Command("TableOfContent")]
        public static CallbackBlock TableOfContent(CompilerState state)
        {
            return new CallbackBlock(() =>
            {
                var entries = state.Document.Variable(TableVariables.TableOfContent);

                var region = new StackPanel() { Orientation = Orientation.Vertical };

                foreach (var entry in entries)
                {
                    region.Elements.Add(entry);
                }

                return region;
            });
        }

        [Command("Heading")]
        public static Paragraph Heading(CompilerState state, Argument<int> level, Argument<Paragraph> content)
        {
            var lvl = level.Value;
            ResetNumbering(state.Document, lvl);
            var pretext = CreatePretext(state.Document, lvl);
            var paragraph = content.Value;
            var entry = new TableElement(pretext, lvl, paragraph.Clone(), paragraph);
            state.Document.Variable(TableVariables.TableOfContent).Add(entry);
            var text = new TextLeaf
            {
                Content = pretext + " "
            };
            paragraph.Leaves.Insert(0, text);

            return HeadingStar(state, level, content);
        }

        [Command("Heading*")]
        public static Paragraph HeadingStar(CompilerState state, Argument<int> level, Argument<Paragraph> content)
        {
            var lvl = level.Value;
            if (lvl < 1 || lvl > MaxLevel)
            {
                state.Issues.Add(level.Source, CompilerIssueType.Warning, "");
                lvl = Math.Clamp(lvl, 1, MaxLevel);
            }
            var paragraph = content.Value;
            paragraph.FontSize = lvl switch
            {
                1 => 18,
                2 => 16,
                3 => 14,
                _ => 12
            };
            paragraph.Margin = new Thickness(14 - lvl * 2, 0);
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
            StringBuilder sb = new StringBuilder();
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
    }
}
