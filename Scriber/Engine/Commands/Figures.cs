using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;
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
                Margin = new Thickness(12, 0)
            };

            var elements = content.Select(e => e.Value).OfType<DocumentElement>().ToArray();

            panel.Elements.AddRange(elements);

            var caption = elements.OfType<Paragraph>().Where(e => Equals(e.Tag, "caption")).FirstOrDefault();
            
            if (caption != null)
            {
                var copiedCaption = caption.Clone();

                var figureCount = FigureVariables.FigureCaptionCounter.Increment(state.Document);

                var text = new TextLeaf
                {
                    Content = "Figure " + figureCount + ": "
                };
                caption.Leaves.Insert(0, text);

                var figures = TableVariables.TableOfFigures.Get(state.Document);
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
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(12, 0)
                };

                var figures = TableVariables.TableOfFigures.Get(state.Document);
                panel.Elements.AddRange(figures);

                return panel;
            });
        }
    }
}
