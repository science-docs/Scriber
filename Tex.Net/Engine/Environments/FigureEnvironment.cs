using System.Linq;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine.Environments
{
    [Package]
    public static class FigureEnvironment
    {
        [Environment("figure")]
        public static object Figure(CompilerState state, object[] content)
        {
            var section = new Region
            {
                Flexible = true
            };

            var elements = content.OfType<DocumentElement>().ToArray();

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
