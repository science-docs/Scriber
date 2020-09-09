using System.Linq;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Environments
{
    [Package]
    public static class FigureEnvironment
    {
        [Environment("Figure")]
        public static object Figure(CompilerState state, Argument[] content)
        {
            var section = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Flexible = true,
                Margin = new Thickness(12, 0)
            };

            var elements = content.Select(e => e.Value).OfType<DocumentElement>().ToArray();

            section.Elements.AddRange(elements);

            foreach (var caption in elements.OfType<Paragraph>().Where(e => Equals(e.Tag, "caption")))
            {
                var text = new TextLeaf
                {
                    Content = "Figure " + FigureVariables.FigureCaptionCounter.Increment(state.Document) + ": "
                };
                caption.Leaves.Insert(0, text);
            }

            return section;
        }
    }
}
