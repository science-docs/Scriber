using System.Collections.Generic;
using System.Linq;
using Scriber.Language;
using Scriber.Layout.Document;
using Scriber.Logging;
using Scriber.Util;

namespace Scriber.Engine
{
    public static class Compiler
    {
        public static CompilerResult Compile(IEnumerable<Element> elements)
        {
            return Compile(elements, null);
        }

        public static CompilerResult Compile(IEnumerable<Element> elements, Logger? logger)
        {
            var state = new CompilerState();

            if (logger != null)
            {
                state.Issues.CollectionChanged += (_, e) => IssuesChanged(logger, e);
            }

            foreach (var element in elements)
            {
                Execute(state, element);
            }

            state.Document.Elements.AddRange(state.Blocks.Current.Objects.OfType<DocumentElement>());

            ResolveCallbacks(state.Document.Elements);

            var result = new CompilerResult(state.Document, state.Issues);
            return result;
        }

        private static void IssuesChanged(Logger logger, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (CompilerIssue issue in e.NewItems.OfType<CompilerIssue>().Where(e => e != null))
                {
                    logger.Log((LogLevel)(int)issue.Type, issue.Message);
                }
            }
        }

        private static void Execute(CompilerState state, Element element)
        {
            if (BlockElement(element))
            {
                state.Blocks.Push();
            }

            foreach (var inline in element.Children)
            {
                Execute(state, inline);
            }

            var obj = state.Execute(element, state.Blocks.Current.Objects.ToArray());

            if (BlockElement(element))
            {
                state.Blocks.Pop();
            }

            if (obj != null && !(obj is Block))
            {
                // basically pass the returned object to the previous environment
                var flattened = obj.ConvertToFlatArray();
                state.Blocks.Current.Objects.AddRange(flattened);
            }
        }

        private static bool BlockElement(Element element)
        {
            // return element.Children.Count > 0;
            return element.Type == ElementType.Block 
                || element.Type == ElementType.Command 
                || element.Type == ElementType.Environment
                || element.Type == ElementType.ExplicitBlock 
                || element.Type == ElementType.ObjectArray 
                || element.Type == ElementType.ObjectCreation 
                || element.Type == ElementType.ObjectField;
        }

        private static void ResolveCallbacks(ElementCollection<DocumentElement> blocks)
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
