using System.Linq;
using Scriber.Layout.Document;

namespace Scriber.Engine.Environments
{
    [Package]
    public static class FigureEnvironment
    {
        [Environment("Figure")]
        public static object Figure(CompilerState state, Argument[] content)
        {
            var section = new Region
            {
                Flexible = true,
                Margin = new Layout.Thickness(12, 0)
            };

            var elements = content.Select(e => e.Value).OfType<DocumentElement>().ToArray();

            section.Elements.AddRange(elements);

            foreach (var caption in elements.OfType<Paragraph>().Where(e => Equals(e.Tag, "caption")))
            {
                var text = new TextLeaf
                {
                    Content = "Figure " + NextCaptionCount(state, "figure") + ": "
                };
                caption.Leaves.Insert(0, text);
            }

            return section;
        }

        private static int NextCaptionCount(CompilerState state, string type)
        {
            var vars = state.Document.Variables;
            var v = vars["captions"][type];

            int value = v.GetValue<int>() + 1;
            v.SetValue(value);

            return value;
        }
    }
}
