using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Tex.Net.Language
{
    using Function = Func<ParserContext, Token, Element, Element?>;

    public static class Parser
    {
        //private readonly static ParserState StateGraph = new ParserState(null, null);
        private readonly static Dictionary<TokenType, Function> ParseActions = new Dictionary<TokenType, Function>();

        static Parser()
        {
            ParseActions[TokenType.Text] = ParseText;
            ParseActions[TokenType.Backslash] = ParseCommand;
            ParseActions[TokenType.CurlyOpen] = ParseCurlyOpen;
            ParseActions[TokenType.CurlyClose] = ParseCurlyClose;
            ParseActions[TokenType.Newline] = ParseNewline;
            ParseActions[TokenType.Percent] = ParsePercent;
            ParseActions[TokenType.BracketOpen] = ParseBracketOpen;
            ParseActions[TokenType.BracketClose] = ParseBracketClose;

            //var textState = new ParserState(TokenType.Text, ParseText);
            //var commandState = new ParserState(TokenType.Backslash, ParseCommand);
            //var curlyCloseState = new ParserState(TokenType.CurlyClose, ParseCurlyClose);
            //var newlineState = new ParserState(TokenType.Newline, ParseNewline);
            //var commentState = new ParserState(TokenType.Percent, ParsePercent);
            //var curlyOpenState = new ParserState(TokenType.CurlyOpen, ParseCurlyOpen);

            //StateGraph.Add(textState, commandState, curlyOpenState, curlyCloseState, newlineState, commentState);
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

        private static Element? ParseNewline(ParserContext context, Token token, Element previous)
        {
            context.Comment = false;
            context.LastLine.Clear();
            context.LastLine.AddRange(context.CurrentLine);
            context.CurrentLine.Clear();
            if (IsLineMixed(context.LastLine) && !context.Newline)
            {
                context.Newline = true;
                return AppendText(previous, context.Parents.Peek(), new Token(" ", token.Index));
            }
            else if (context.Newline)
            {
                context.Newline = false;
                return new Element(context.Parents.Peek(), ElementType.Paragraph, token.Index);
            }
            else
            {
                return null;
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

        private static Element ParseBracketOpen(ParserContext context, Token token, Element previous)
        {
            if (previous.Type == ElementType.Command && context.Parents.Peek().Type != ElementType.Command)
            {
                context.Parents.Push(previous);
                var element = new Element(previous, ElementType.Block, token.Index);
                context.Parents.Push(element);
                context.BracketBlock = true;
                return element;
            }
            else
            {
                return AppendText(previous, context.Parents.Peek(), token);
            }
        }

        private static Element? ParseBracketClose(ParserContext context, Token token, Element previous)
        {
            if (context.BracketBlock)
            {
                context.BracketBlock = false;
                context.Parents.Pop();
                return null;
            }
            else
            {
                return AppendText(previous, context.Parents.Peek(), token);
            }
        }

        private static Element ParseCurlyOpen(ParserContext context, Token token, Element previous)
        {
            // if we are in optional arguments
            if (context.BracketBlock)
            {
                return AppendText(previous, context.Parents.Peek(), token);
            }
            else
            {
                Element parent;

                if (previous.Type == ElementType.Command && context.Parents.Peek().Type != ElementType.Command)
                {
                    context.Parents.Push(previous);
                }

                parent = context.Parents.Peek();
                var element = new Element(parent, ElementType.Block, token.Index);
                context.Parents.Push(element);
                return element;
            }
        }

        private static Element? ParseCurlyClose(ParserContext context, Token token, Element previous)
        {
            if (context.BracketBlock)
            {
                return AppendText(previous, context.Parents.Peek(), token);
            }
            else
            {
                context.Parents.Pop();
                return null;
            }
        }

        public static Element[] Parse(IEnumerable<Token> tokens)
        {
            return ParseInternal(tokens).ToArray();
        }

        private static IEnumerable<Element> ParseInternal(IEnumerable<Token> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            var items = tokens.ToArray();

            if (items.Length == 0)
                yield break;

            Element cur = new Element(null, ElementType.Block, 0);
            var context = new ParserContext();
            context.Parents.Push(cur);
            bool returnedLast = false;

            foreach (var token in items)
            {
                var action = ParseActions[token.Type];
                bool isNewline = token.Type == TokenType.Newline;
                bool isCurlyOpen = token.Type == TokenType.CurlyOpen;
                bool isBracketOpen = token.Type == TokenType.BracketOpen;

                if (!isCurlyOpen && !isBracketOpen && context.Parents.Peek().Type == ElementType.Command)
                {
                    context.Parents.Pop();
                }

                if (!isNewline && context.Comment)
                {
                    cur.StringBuilder?.Append(token.Content);
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
                        cur = new Element(null, ElementType.Block, token.Index);
                        context.Parents.Push(cur);
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
                    // if at top parent
                    if (context.Parents.Count == 1)
                    {
                        context.CurrentLine.Add(element);
                    }
                    cur = element;
                    returnedLast = false;
                }
            }

            if (!returnedLast)
            {
                cur = TopParent(cur);
                BuildContent(cur);
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

        private static void BuildContent(Element element)
        {
            if (element.StringBuilder != null)
            {
                element.Content = element.StringBuilder.ToString();
                element.StringBuilder = null;
                element.Length = element.Content.Length;
            }

            foreach (var inline in element.Inlines)
            {
                BuildContent(inline);
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

        private static bool IsLineMixed(IEnumerable<Element> elements)
        {
            return elements.Any(e => e.Type == ElementType.Text);
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
    }
}
