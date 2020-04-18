using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tex.Net.Language
{
    using Function = Func<ParserContext, Token, Element, Element>;

    public static class Parser
    {        
        private readonly static ParserState StateGraph = new ParserState(null, ParserStateType.Start);
        private readonly static Dictionary<TokenType, Function> ParseActions = new Dictionary<TokenType, Function>();

        static Parser()
        {
            ParseActions[TokenType.Text] = ParseText;
            ParseActions[TokenType.Backslash] = ParseCommand;
            ParseActions[TokenType.CurlyOpen] = ParseCurlyOpen;
            ParseActions[TokenType.CurlyClose] = ParseCurlyClose;
            ParseActions[TokenType.Newline] = ParseNewline;
            ParseActions[TokenType.Percent] = ParsePercent;

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

        private static Element ParsePercent(ParserContext context, Token token, Element element)
        {
            context.Comment = true;
            return new Element(element.Parent, ElementType.Comment, token.Index) { StringBuilder = new StringBuilder() };
        }

        private static Element ParseText(ParserContext context, Token token, Element element)
        {
            return AppendText(element, FindParent(element), token);
        }

        private static Element ParseNewline(ParserContext context, Token token, Element element)
        {
            context.Comment = false;
            if (context.Newline)
            {
                context.Newline = false;
                return null;
            }
            else
            {
                context.Newline = true;
                return AppendText(element, FindParent(element), new Token(" ", token.Index));
            }
        }

        private static Element ParseCommand(ParserContext context, Token token, Element element)
        {
            var parent = FindParent(element);
            var command = new Element(parent, ElementType.Command, token.Index)
            {
                Content = token.Content.Substring(1)
            };
            return command;
        }

        private static Element ParseCurlyOpen(ParserContext context, Token token, Element element)
        {
            if (element.Type == ElementType.Command)
            {
                if (string.IsNullOrEmpty(element.Content))
                {
                    var block = new Element(element.Parent, ElementType.Text, token.Index)
                    {
                        Content = token.Content
                    };
                    return block;
                }
                else
                {
                    var block = new Element(element, ElementType.Block, token.Index);
                    element.CommandArguments.Add(block);
                    return block;
                }
            }
            else
            {
                var block = new Element(element.Parent, ElementType.Block, token.Index);
                return block;
            }
        }

        private static Element ParseCurlyClose(ParserContext context, Token token, Element element)
        {
            if (element.Type == ElementType.Command && string.IsNullOrEmpty(element.Content))
            {
                var block = new Element(element.Parent, ElementType.Text, token.Index)
                {
                    Content = token.Content
                };
                return block;
            }

            return element.Parent;
        }

        public static IEnumerable<Element> Parse(IEnumerable<Token> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            var items = tokens.ToArray();

            if (items.Length == 0)
                yield break;

            Element cur = new Element(null, ElementType.Block, 0);
            Element last = null;
            var context = new ParserContext();
            var state = StateGraph;
            bool returnedLast = false;

            foreach (var token in items)
            {
                //if (!state.Next.TryGetValue(token.Type, out state))
                //{
                //    throw new Exception();
                //}

                var action = ParseActions[token.Type];
                bool isNewline = action == ParseNewline;

                if (!isNewline && context.Comment)
                {
                    cur.StringBuilder.Append(token.Content);
                    continue;
                }

                var element = action(context, token, cur);

                if (element == null)
                {
                    cur = TopParent(cur);
                    BuildContent(cur);
                    yield return cur;
                    last = cur;
                    cur = new Element(null, ElementType.Block, token.Index);
                    last.NextSibling = cur;
                    cur.PreviousSibling = last;
                    returnedLast = true;
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

        private static Element AppendText(Element element, Element parent, Token token)
        {
            var textItem = element.Type == ElementType.Text ? element : new Element(parent, ElementType.Text, token.Index);

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

        private class ParserState
        {
            public ParserStateType Type { get; set; } = ParserStateType.Start;
            public Dictionary<TokenType, ParserState> Next { get; } = new Dictionary<TokenType, ParserState>();
            public Func<Token, Element, Element> ParseAction { get; set; }

            public ParserState(TokenType? tokenType, ParserStateType type)
            {
                Type = type;
                if (tokenType != null)
                {
                    Next.Add(tokenType.Value, this);
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
