using Scriber.Engine.Converter;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Acronyms
    {
        [Command("Acronyms")]
        public static StackPanel ListOfAcronyms(CompilerState state, DynamicDictionary acronyms)
        {
            var acro = acronyms.GetContents();
            var stringConverter = new ParagraphConverter();
            var list = AcronymVariables.Acronyms.Get(state.Document)!;
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            foreach (var ac in acro)
            {
                var key = ac.Key;
                var value = ac.Value;

                if (value is Paragraph paragraph)
                {
                    var stringValue = (stringConverter.Convert(paragraph, typeof(string)) as string)!;
                    list.Add((key, stringValue));

                    var grid = new Grid();
                    grid.Rows.Add(new GridLength(GridUnit.Auto, 0));
                    grid.Columns.Add(new GridLength(GridUnit.Star, 1));
                    grid.Columns.Add(new GridLength(GridUnit.Auto, 0));

                    grid.Set(Paragraph.FromText(key), 0, 0);
                    grid.Set(Paragraph.FromText(stringValue), 0, 1);

                    stackPanel.Elements.Add(grid);
                }
            }

            return stackPanel;
        }

        [Command("Acronym")]
        public static ITextLeaf SingleAcronym(CompilerState state, string name)
        {
            var list = AcronymVariables.Acronyms.Get(state.Document)!;
            var usedAcronyms = AcronymVariables.UsedAcronyms.Get(state.Document)!;
            var pair = list.FirstOrDefault(e => e.name.ToLowerInvariant() == name.ToLowerInvariant());

            if (pair != default((string, string)))
            {
                string text = pair.name;

                if (usedAcronyms.Add(pair.name))
                {
                    text = pair.full;
                }

                return new TextLeaf(text);
            }
            else
            {
                return new TextLeaf("???");
            }
        }
    }
}
