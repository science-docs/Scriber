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
                var entries = state.Document.Variable(TableVariables.TableOfContent)!;

                var region = new StackPanel() { Orientation = Orientation.Vertical };

                foreach (var entry in entries)
                {
                    region.Elements.Add(entry);
                }

                return region;
            });
        }

        [Command("Header")]
        public static Paragraph Header(CompilerState state, int level, Paragraph content)
        {
            ResetNumbering(state.Document, level);
            var pretext = CreatePretext(state.Document, level);
            var entry = new TableElement(pretext, level, content.Clone(), content);
            state.Document.Variable(TableVariables.TableOfContent)!.Add(entry);
            var text = new TextLeaf
            {
                Content = pretext + " "
            };
            content.Leaves.Insert(0, text);

            return HeaderStar(level, content);
        }

        [Command("Header*")]
        public static Paragraph HeaderStar(int level, Paragraph content)
        {
            if (level < 1 || level > MaxLevel)
            {

            }

            content.FontSize = level switch
            {
                1 => 18,
                2 => 16,
                3 => 14,
                _ => 12
            };
            content.Margin = new Thickness(14 - level * 2, 0);
            return content;
        }

        [Command("Section")]
        public static Paragraph Section(CompilerState state, Paragraph content)
        {
            return Header(state, 1, content);
        }

        [Command("Section*")]
        public static Paragraph SectionStar(Paragraph content)
        {
            return HeaderStar(1, content);
        }

        [Command("Subsection")]
        public static Paragraph Subsection(CompilerState state, Paragraph content)
        {
            return Header(state, 2, content);
        }

        [Command("Subsection*")]
        public static Paragraph SubsectionStar(Paragraph content)
        {
            return HeaderStar(2, content);
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
