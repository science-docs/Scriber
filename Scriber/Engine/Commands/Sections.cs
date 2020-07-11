using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Layout;
using Scriber.Layout.Document;

namespace Scriber.Engine.Commands
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

        [Command("TableOfContent")]
        public static CallbackBlock TableOfContent(CompilerState state)
        {
            return new CallbackBlock(() =>
            {
                var vars = state.Document.Variables;
                var entries = GetEntryTable(vars);

                var region = new StackPanel() { Orientation = Orientation.Vertical };

                foreach (var entry in entries)
                {
                    region.Elements.Add(new TableElement(entry.Preamble, entry.Level, entry.Content, entry.Reference));
                }

                return region;
            });
        }

        [Command("Section")]
        public static Paragraph Section(CompilerState state, Paragraph content)
        {
            const int level = 1;

            var vars = state.Document.Variables;
            content.FontSize = 18;
            var pretext = CreatePretext(vars, level);
            var entry = new TableEntry(pretext, level, content.Clone(), content);
            GetEntryTable(vars).Add(entry);
            ResetNumbering(vars, level);
            var text = new TextLeaf
            {
                Content = pretext + " "
            };
            content.Leaves.Insert(0, text);

            return SectionStar(state, content);
        }

        [Command("Section*")]
        public static Paragraph SectionStar(CompilerState state, Paragraph content)
        {
            content.FontSize = 18;
            content.Margin = new Thickness(12, 0);
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

        private static List<TableEntry> GetEntryTable(DocumentVariable variables)
        {
            var toc = variables[DocumentVariables.TableOfContent]["entries"];
            var list = toc.GetValue<List<TableEntry>>();
            if (list == null)
            {
                list = new List<TableEntry>();
                toc.SetValue(list);
            }
            return list;
        }
    }

    public class TableEntry
    {
        public string? Preamble { get; }
        public int Level { get; }
        public Paragraph Reference { get; }
        public Paragraph Content { get; }
        public PageReference Page { get; }

        public TableEntry(string? preamble, int level, Paragraph content, Paragraph reference)
        {
            Content = content;
            Level = level;
            Preamble = preamble;
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            Page = new PageReference(reference);
        }
    }
}
