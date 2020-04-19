using System.Collections.Generic;
using System.Linq;
using Tex.Net.Language;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine
{
    public static class Compiler
    {
        public static CompilerResult Compile(Element element)
        {
            var state = new CompilerState();
            var args = new List<object>();
            Execute(state, element, args);

            state.Document.Elements.AddRange(state.Environments.Current.Objects.OfType<Block>());
            var result = new CompilerResult(state.Document);
            result.Issues.AddRange(state.Issues);
            return result;
        }

        private static void Execute(CompilerState state, Element element, List<object> arguments)
        {
            var ownArgs = new List<object>();

            if (element.Inline != null)
            {
                Execute(state, element.Inline, ownArgs);
            }

            var obj = state.Execute(element, ownArgs.ToArray());

            if (obj != null)
            {
                arguments.Add(obj);
            }

            if (element.NextSibling != null)
            {
                Execute(state, element.NextSibling, arguments);
            }
        }
    }
}
