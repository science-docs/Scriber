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
            ResolveCallbacks(state.Document.Elements);
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

            if (MustCreateEnvironment(element))
            {
                state.Environments.Push();
            }

            var obj = state.Execute(element, ownArgs.ToArray());
            
            if (MustDestroyEnvironment(element))
            {
                state.Environments.Pop();
            }

            if (obj != null)
            {
                arguments.Add(obj);
            }

            if (element.NextSibling != null)
            {
                Execute(state, element.NextSibling, arguments);
            }
        }

        private static bool MustCreateEnvironment(Element element)
        {
            return element.Type == ElementType.Block || (element.Type == ElementType.Command && element.Content == "begin");
        }

        private static bool MustDestroyEnvironment(Element element)
        {
            return element.Type == ElementType.Block || (element.Type == ElementType.Command && element.Content == "end");
        }

        private static void ResolveCallbacks(DocumentElementCollection<Block> blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                if (block is CallbackBlock callbackBlock)
                {
                    blocks[i] = callbackBlock.Callback();
                }
            }
        }
    }
}
