using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Tex.Net.Language
{
    using Function = Func<ParserContext, Token, Element, Element>;

    public static class Parser
    {
        private readonly static ParserState StateGraph = new ParserState(null, null);
        private readonly static Dictionary<TokenType, Function> ParseActions = new Dictionary<TokenType, Function>();

        static Parser()
        {
            ParseActions[TokenType.Text] = ParseText;
            ParseActions[TokenType.Backslash] = ParseCommand;
            ParseActions[TokenType.CurlyOpen] = ParseCurlyOpen;
            ParseActions[TokenType.CurlyClose] = ParseCurlyClose;
            ParseActions[TokenType.Newline] = ParseNewline;
            ParseActions[TokenType.Percent] = ParsePercent;

            var textState = new ParserState(TokenType.Text, ParseText);
            var commandState = new ParserState(TokenType.Backslash, ParseCommand);
            var curlyCloseState = new ParserState(TokenType.CurlyClose, ParseCurlyClose);
            var newlineState = new ParserState(TokenType.Newline, ParseNewline);
            var commentState = new ParserState(TokenType.Percent, ParsePercent);
            var curlyOpenState = new ParserState(TokenType.CurlyOpen, ParseCurlyOpen);

            StateGraph.Add(textState, commandState, curlyOpenState, curlyCloseState, newlineState, commentState);

            //var textState = new ParserState(TokenType.Text, ParserStateType.Text)
            //{
            //    ParseAction = ParseText
            //};
            //var commandState = new ParserState(TokenType.Backslash, ParserStateType.Command)
            //{
            //    ParseAction = ParseCommand
            //};
            //var curlyOpenState = new ParserState(TokenType.CurlyOpen, ParserStateType.Text)
            //{
            //    ParseAction = ParseCurlyOpen
            //};
            //var curlyCloseState = new ParserState(TokenType.CurlyClose, ParserStateType.Text)
            //{
            //    ParseAction = ParseCurlyClose
            //};
            //StateGraph.Next.Add(TokenType.Text, textState);
            //StateGraph.Next.Add(TokenType.Backslash, commandState);
            //textState.Next.Add(TokenType.Backslash, commandState);
            //commandState.Next.Add(TokenType.Text, textState);
        }

        private static Element ParsePercent(ParserContext context, Token token, Element previous)
        {
            context.Comment = true;
            return new Element(context.Parents.Peek(), ElementType.Comment, token.Index) { StringBuilder = new StringBuilder() };
        }

        private static Element ParseText(ParserContext context, Token token, Element previous)
        {
            return AppendText(previous, context.Parents.Peek(), token);
        }

        private static Element ParseNewline(ParserContext context, Token token, Element previous)
        {
            context.Comment = false;
            if (context.Newline)
            {
                context.Newline = false;
                context.Parents.Clear();
                return null;
            }
            else
            {
                context.Newline = true;
                return AppendText(previous, context.Parents.Peek(), new Token(" ", token.Index));
            }
        }

        private static Element ParseCommand(ParserContext context, Token token, Element previous)
        {
            var command = new Element(context.Parents.Peek(), ElementType.Command, token.Index)
            {
                Content = token.Content.Substring(1)
            };
            return command;
        }

        //private static Element ParseCurlyOpenCommand(ParserContext context, Token token, Element previous)
        //{
        //    context.Parents.Push(previous);
        //    return null;
        //}

        private static Element ParseCurlyOpen(ParserContext context, Token token, Element previous)
        {
            Element parent;
            if (previous.Type == ElementType.Command)
            {
                parent = previous;
            }
            else
            {
                parent = context.Parents.Peek();
            }
            var element = new Element(parent, ElementType.Block, token.Index);
            context.Parents.Push(element);
            return element;
        }

        private static Element ParseCurlyClose(ParserContext context, Token token, Element previous)
        {
            //if (element.Type == ElementType.Command && string.IsNullOrEmpty(element.Content))
            //{
            //    var block = new Element(element.Parent, ElementType.Text, token.Index)
            //    {
            //        Content = token.Content
            //    };
            //    return block;
            //}

            //return element.Parent;

            context.Parents.Pop();
            return null;
        }

        public static Element Parse(IEnumerable<Token> tokens)
        {
            return ParseInternal(tokens).ToArray().FirstOrDefault();
        }

        private static IEnumerable<Element> ParseInternal(IEnumerable<Token> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            var items = tokens.ToArray();

            if (items.Length == 0)
                yield break;

            Element cur = new Element(null, ElementType.Block, 0);
            Element last = null;
            var context = new ParserContext();
            context.Parents.Push(cur);
            var state = StateGraph;
            bool returnedLast = false;

            foreach (var token in items)
            {
                if (!state.Next.TryGetValue(token.Type, out state))
                {
                    state = StateGraph.Next[token.Type];
                }

                var action = state.ParseAction;
                bool isNewline = action == ParseNewline;

                if (!isNewline && context.Comment)
                {
                    cur.StringBuilder.Append(token.Content);
                    continue;
                }
                else if (!isNewline)
                {
                    context.Newline = false;
                }

                var element = action(context, token, cur);

                if (element == null)
                {
                    if (!context.Parents.TryPeek(out var top))
                    {
                        cur = TopParent(cur);
                        BuildContent(cur);
                        yield return cur;
                        last = cur;
                        cur = new Element(null, ElementType.Block, token.Index);
                        context.Parents.Push(cur);
                        last.NextSibling = cur;
                        cur.PreviousSibling = last;
                        returnedLast = true;
                    }
                    else
                    {
                        cur = top;
                        returnedLast = false;
                    }
                }
                else
                {
                    cur = element;
                    returnedLast = false;
                }
            }

            if (!returnedLast)
            {
                cur = TopParent(cur);
                BuildContent(cur);
                last?.SetNextSibling(cur);
                yield return cur;
            }
        }

        private static Element TopParent(Element element)
        {
            if (element.Parent == null)
            {
                return element;
            }
            else
            {
                return TopParent(element.Parent);
            }
        }

        private static Element FindParent(Element element)
        {
            var parent = element.Parent;
            if (parent == null || EmptyBlock(element))
            {
                parent = element;
            }
            return parent;
        }

        private static bool EmptyBlock(Element element)
        {
            return element.Type == ElementType.Block && element.Inline == null;
        }

        private static void BuildContent(Element element)
        {
            if (element.StringBuilder != null)
            {
                element.Content = element.StringBuilder.ToString();
                element.StringBuilder = null;
                element.Length = element.Content.Length;
            }
            if (element.Inline != null)
            {
                BuildContent(element.Inline);
            }
            if (element.NextSibling != null)
            {
                BuildContent(element.NextSibling);
            }
        }

        private static Element AppendText(Element previous, Element parent, Token token)
        {
            var textItem = previous.Type == ElementType.Text ? previous : new Element(parent, ElementType.Text, token.Index);

            if (textItem.StringBuilder == null)
            {
                textItem.StringBuilder = new StringBuilder(token.Content);
            }
            else
            {
                textItem.StringBuilder.Append(token.Content);
            }

            return textItem;
        }

        [DebuggerDisplay("{DebuggerDisplay}")]
        private class ParserState
        {
            public TokenType? Token { get; set; }
            public Dictionary<TokenType, ParserState> Next { get; } = new Dictionary<TokenType, ParserState>();
            public Function ParseAction { get; set; }

            public ParserState(TokenType? tokenType, Function parseAction, params ParserState[] nextStates)
            {
                Token = tokenType;
                ParseAction = parseAction;
                Add(nextStates);
            }

            public void Add(params ParserState[] states)
            {
                foreach (var state in states)
                {
                    if (state.Token.HasValue)
                    {
                        Next.Add(state.Token.Value, state);
                    }
                }
            }

            string DebuggerDisplay
            {
                get
                {
                    return Token == null ? "None" : $"{Token} {ParseAction}";
                }
            }
        }

        private enum ParserStateType
        {
            Start = 0,
            Text,
            Command

        }
    }
}
