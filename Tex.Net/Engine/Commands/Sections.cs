using System.Text;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine.Commands
{
    [Package]
    public static class Sections
    {
        private const int MaxLevel = 3;
        private const string Level1 = "section";
        private const string Level2 = "subsection";
        private const string Level3 = "subsubsection";

        private const string Number = "number";

        private readonly static string[] Levels = new string[]
        {
            Level1, Level2, Level3
        };

        [Command("section")]
        public static Paragraph Section(CompilerState state, Paragraph content)
        {
            const int level = 1;

            var vars = state.Document.Variables;
            var pretext = CreatePretext(vars, level);
            var entry = new TableEntry
            {
                Content = content,
                Preamble = pretext,
                Page = new PageReference(content)
            };
            vars["toc"][pretext].SetValue(entry);
            ResetNumbering(vars, level);
            var text = new Layout.Document.Text
            {
                Content = pretext
            };
            content.Leaves.Insert(0, text);

            return SectionStar(state, content);
        }

        [Command("section*")]
        public static Paragraph SectionStar(CompilerState state, Paragraph content)
        {
            content.FontSize = 16;
            content.Margin = new Layout.Thickness(12, 0);
            return content;
        }

        private static string CreatePretext(DocumentVariable variables, int level)
        {
            StringBuilder sb = new StringBuilder();

            string levelString = Levels[level - 1];

            var current = variables[levelString][Number];
            var value = current.GetValue<int>() + 1;
            current.SetValue(value);

            for (int i = 0; i < level; i++)
            {
                sb.Append(variables[levelString][Number].GetValue<int>());
                if (i < level - 1)
                {
                    sb.Append(".");
                }
            }

            return sb.ToString();
        }

        private static void ResetNumbering(DocumentVariable variables, int startLevel)
        {
            for (int i = startLevel; i < MaxLevel; i++)
            {
                var levelString = Levels[startLevel];
                variables[levelString][Number].SetValue(0);
            }
        }

        
    }

    public class TableEntry
    {
        public string Preamble { get; set; }
        public Paragraph Content { get; set; }
        public PageReference Page { get; set; }
    }
}
