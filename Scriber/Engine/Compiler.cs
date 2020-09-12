using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Scriber.Language;
using Scriber.Layout.Document;
using Scriber.Logging;

namespace Scriber.Engine
{
    public static class Compiler
    {
        public static CompilerResult Compile(Context context, IEnumerable<Element> elements)
        {
            var state = new CompilerState(context);
            state.Issues.CollectionChanged += (_, e) => IssuesChanged(context.Logger, e);

            foreach (var element in elements)
            {
                Execute(state, element);
            }

            state.Document.Elements.AddRange(state.Blocks.Current.Objects.Select(e => e.Value).OfType<DocumentElement>());

            ResolveCallbacks(state.Document.Elements);

            var result = new CompilerResult(state.Document, state.Issues);
            return result;
        }

        public static void Compile(CompilerState state, IEnumerable<Element> elements)
        {
            var block = state.Blocks.Push();
            foreach (var element in elements)
            {
                Execute(state, element);
            }
            state.Blocks.Pop();
            // As the @Include Command is nested 1 block deep,
            // we need to add the elements to parent block
            state.Blocks.Peek(1).Objects.AddRange(block.Objects);
        }

        private static void IssuesChanged(Logger logger, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
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

            if (obj != null)
            {
                // basically pass the returned object to the previous environment
                var flattened = obj.Flatten();
                if (IsEnvironmentArgument(element))
                {
                    flattened = new Argument[] { new Argument(element, flattened) };
                }
                state.Blocks.Current.Objects.AddRange(flattened);
            }
        }

        private static bool IsEnvironmentArgument(Element element)
        {
            element.Siblings(out var _, out var next);
            return element.Type == ElementType.ExplicitBlock && element.Parent != null && element.Parent.Type == ElementType.Environment && next == null;
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
