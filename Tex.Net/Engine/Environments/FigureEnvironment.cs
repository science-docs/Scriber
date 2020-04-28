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
            var section = new Section
            {
                Flexible = true
            };
            section.Elements.AddRange(content.OfType<DocumentElement>());
            return section;
        }
    }
}
