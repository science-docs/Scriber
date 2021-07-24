using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Scriber.Language;
using Scriber.Language.Syntax;
using Scriber.Layout.Document;
using Scriber.Logging;

namespace Scriber.Engine
{
    public static class Compiler
    {
        public static CompilerState Compile(Context context, IEnumerable<SyntaxNode> elements)
        {
            var state = new CompilerState(context);
            state.Issues.CollectionChanged += (_, e) => IssuesChanged(context.Logger, e);

            foreach (var element in elements)
            {
                if (state.Continue)
                {
                    state.Document.Elements.AddRange(Execute(state, element));
                }
            }
            return state;
        }

        public static void Compile(CompilerState state, IEnumerable<SyntaxNode> elements)
        {
            foreach (var element in elements)
            {
                state.Document.Elements.AddRange(Execute(state, element));
            }
        }

        public static CompilerState Compile(Context context, Resource resource)
        {
            context.ResourceManager.PushResource(resource);
            var tokens = Lexer.Tokenize(resource.GetContentAsString());
            var parserResult = Parser.Parse(tokens, resource, context.Logger);
            var result = Compile(context, parserResult.Nodes);
            context.ResourceManager.PopResource();
            return result;
        }

        public static void Compile(CompilerState state, Resource resource)
        {
            state.Context.ResourceManager.PushResource(resource);
            var tokens = Lexer.Tokenize(resource.GetContentAsString());
            var parserResult = Parser.Parse(tokens, resource, state.Context.Logger);
            Compile(state, parserResult.Nodes);
            state.Context.ResourceManager.PopResource();
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

        private static IEnumerable<DocumentElement> Execute(CompilerState state, SyntaxNode element)
        {
            var obj = state.Execute(element);

            var elements = new List<DocumentElement>();

            if (obj != null)
            {
                var flattened = obj.Flatten();
                return flattened.Select(e => e.Value).OfType<DocumentElement>();
            }
            else
            {
                return Array.Empty<DocumentElement>();
            }
        }
    }
}
