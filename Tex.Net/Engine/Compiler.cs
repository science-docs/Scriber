using System.Collections.Generic;
using System.Linq;
using Tex.Net.Language;
using Tex.Net.Layout.Document;
using Tex.Net.Util;

namespace Tex.Net.Engine
{
    public static class Compiler
    {
        public static CompilerResult Compile(IEnumerable<Element> elements)
        {
            var state = new CompilerState();
            var args = new List<object>();

            foreach (var element in elements)
            {
                Execute(state, element, args);
            }

            state.Document.Elements.AddRange(state.Environments.Current.Objects.OfType<Block>());
            ResolveCallbacks(state.Document.Elements);
            var result = new CompilerResult(state.Document);
            result.Issues.AddRange(state.Issues);
            return result;
        }

        private static void Execute(CompilerState state, Element element, List<object> arguments)
        {
            var ownArgs = new List<object>();

            if (element.Type == ElementType.Block)
            {
                state.Environments.Push();
            }

            foreach (var inline in element.Inlines)
            {
                Execute(state, inline, ownArgs);
            }

            var obj = state.Execute(element, ownArgs.ToArray());

            if (element.Type == ElementType.Block)
            {
                state.Environments.Pop();
            }

            if (obj != null)
            {
                // basically pass the returned object to the previous environment
                var flattened = obj.ConvertToFlatArray();
                state.Environments.Current.Objects.AddRange(flattened);
                arguments.Clear();
                arguments.AddRange(state.Environments.Current.Objects);
                //arguments.AddRange(flattened);
            }

            

            //if (element.NextSibling != null)
            //{
            //    Execute(state, element.NextSibling, arguments);
            //}
        }

        private static bool MustCreateEnvironment(Element element, out string name)
        {
            if (element.Type == ElementType.Block)
            {
                name = "{none}";
                return true;
            }
            else if (element.Type == ElementType.Command && element.Content == "begin")
            {
                name = EnvironmentName(element);
                return true;
            }
            else
            {
                name = null;
                return false;
            }
        }

        private static string EnvironmentName(Element command)
        {
            return command.Inlines.First().Inlines.First().Content;
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
