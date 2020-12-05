﻿using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Acronyms
    {
        [Command("Acronyms")]
        public static StackPanel ListOfAcronyms(CompilerState state, Argument<DynamicDictionary> acronyms)
        {
            var acro = acronyms.Value.GetContents();
            var list = AcronymVariables.Acronyms.Get(state.Document);
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            if (list.Count > 0)
            {
                state.Issues.Add(acronyms.Source, CompilerIssueType.Warning, "");
            }

            foreach (var ac in acro.OrderBy(e => e.Key))
            {
                var key = ac.Key;
                var value = ac.Value;

                if (value is Paragraph paragraph)
                {
                    list.Add((key, paragraph));

                    var grid = new Grid();
                    grid.Rows.Add(new GridLength(GridUnit.Auto, 0));
                    grid.Columns.Add(new GridLength(GridUnit.Star, 1));
                    grid.Columns.Add(new GridLength(GridUnit.Auto, 0));

                    grid.Set(Paragraph.FromText(key), 0, 0);
                    grid.Set(paragraph, 0, 1);

                    stackPanel.Elements.Add(grid);
                }
            }

            return stackPanel;
        }

        [Command("Acronym")]
        public static IEnumerable<Leaf> SingleAcronym(CompilerState state, string name)
        {
            var list = AcronymVariables.Acronyms.Get(state.Document);
            var usedAcronyms = AcronymVariables.UsedAcronyms.Get(state.Document);
            var pair = list.FirstOrDefault(e => e.name.ToLowerInvariant() == name.ToLowerInvariant());

            if (pair != default((string, Paragraph)))
            {
                var paragraph = Paragraph.FromLeaves(new ReferenceLeaf(pair.full, pair.name));

                if (usedAcronyms.Add(pair.name))
                {
                    paragraph = pair.full;
                }

                return paragraph.Leaves;
            }
            else
            {
                return new Leaf[] { new TextLeaf("???") };
            }
        }
    }
}
