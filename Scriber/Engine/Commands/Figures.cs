using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Localization;
using Scriber.Variables;
using System;
using System.Linq;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Figures
    {
        [Command("Figure")]
        public static StackPanel Figure(CompilerState state, Argument<DocumentElement>[] content)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Flexible = true,
                Tag = "figure"
            };

            var elements = content.Select(e => e.Value).OfType<DocumentElement>().ToArray();

            panel.Elements.AddRange(elements);

            var caption = elements.OfType<Paragraph>().Where(e => "caption".Equals(e.Tag, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            
            if (caption != null)
            {
                var copiedCaption = caption.Clone();
                copiedCaption.Tag = "p";

                var figureCount = FigureVariables.FigureCaptionCounter.Increment(state.Document);

                var text = new TextLeaf
                {
                    Content = $"{state.Document.Locale.GetTerm(Term.Figure)} {figureCount}: "
                };
                caption.Leaves.Insert(0, text);

                var figures = TableVariables.TableOfFigures.Get(state.Document) ?? throw new InvalidOperationException();
                var tableElement = new TableElement(figureCount.ToString(), 1, copiedCaption, panel);
                figures.Add(tableElement);
            }

            return panel;
        }

        [Command("TableOfFigures")]
        public static CallbackBlock TableOfFigures(CompilerState state)
        {
            return new CallbackBlock(() =>
            {
                var panel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                var figures = TableVariables.TableOfFigures.Get(state.Document) ?? throw new InvalidOperationException();
                panel.Elements.AddRange(figures);

                return panel;
            });
        }
    }
}
