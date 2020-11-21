using Scriber.Language.Syntax;
using Scriber.Logging;
using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scriber.Language
{
    public static class Parser
    {
        private static SyntaxNode ParsePercent(ParserContext context)
        {
            var token = context.Tokens.Dequeue()!;
            return new PercentSyntax
            {
                Span = token.Span
            };
        }

        private static SyntaxNode ParseText(ParserContext context, bool inArguments = false)
        {
            var token = context.Tokens.Peek();
            int start = token!.Index;
            int line = token!.Line;
            int last = start + token!.Length;
            bool whitespace = false;
            var sb = new StringBuilder();

            while (token != null)
            {
                if (token.Type == TokenType.Newline)
                {
                    context.Tokens.Dequeue();
                    var nextNewline = context.Tokens.Peek()?.Type == TokenType.Newline;
                    if (nextNewline)
                    {
                        break;
                    }
                    if (!whitespace)
                    {
                        sb.Append(' ');
                    }
                    
                    whitespace = true;
                }
                else if (token.Type == TokenType.Whitespace)
                {
                    context.Tokens.Dequeue();
                    if (!whitespace)
                    {
                        sb.Append(' ');
                    }

                    whitespace = true;
                }
                else if (inArguments && token.Type == TokenType.ParenthesesClose)
                {
                    break;
                }
                else if (IsTextToken(token.Type))
                {
                    context.Tokens.Dequeue();
                    whitespace = false;
                    sb.Append(token.Content);
                }
                else
                {
                    break;
                }

                last = token.Index + token.Length;
                token = context.Tokens.Peek();
            }

            return new TextSyntax
            {
                Span = new TextSpan(start, last - start, line),
                Text = sb.ToString()
            };
        }

        private static bool IsTextToken(TokenType type)
        {
            return type == TokenType.Text || type == TokenType.Backslash || type == TokenType.ParenthesesOpen || type == TokenType.ParenthesesClose || type == TokenType.BracketOpen || type == TokenType.BracketClose;
        }

        private static SyntaxNode ParseSingleLineComment(ParserContext context)
        {
            var token = context.Tokens.Peek();
            int start = token!.Index;
            int line = token!.Line;
            int last = start + token!.Length;

            var sb = new StringBuilder();

            while (token != null)
            {
                if (token.Type == TokenType.Newline)
                {
                    context.Tokens.Dequeue();
                    break;
                }
                else
                {
                    sb.Append(token.Content);
                }
                last = token.Index + token.Length;
                token = context.Tokens.Dequeue();
            }

            return new CommentSyntax
            {
                Span = new TextSpan(start, last - start, line),
                Comment = sb.ToString()
            };
        }

        private static CommandSyntax ParseCommand(ParserContext context)
        {
            var token = context.Tokens.Dequeue();
            int start = token!.Index;
            int line = token!.Line;
            int last = start + token!.Length;

            var command = new CommandSyntax
            {
                Name = ParseName(context)
            };

            var nextTokenParentheses = context.Tokens.Peek()?.Type == TokenType.ParenthesesOpen;
            if (nextTokenParentheses)
            {
                command.Arguments = ParseArguments(context);
            }
            else
            {
                command.Arguments = new ListSyntax<ArgumentSyntax>();
            }

            if (context.Tokens.SkipSearchFor(e => e.Type == TokenType.Whitespace || e.Type == TokenType.Newline, e => e.Type == TokenType.CurlyOpen))
            {
                command.EnvironmentBlock = ParseExplicitBlock(context);
            }

            if (command.Arity > 0)
            {
                command.Span = new TextSpan(start, command.Arguments!.Children[^1].Span.End - start, line);
            }
            else
            {
                command.Span = new TextSpan(start, command.Name.Span.End - start, line);
            }

            return command;
        }

        private static NameSyntax ParseName(ParserContext context)
        {
            var token = context.Tokens.Peek();
            if (token == null)
            {
                throw new ParserException();
            }

            if (token.Type != TokenType.Text)
            {
                throw new ParserException();
            }

            context.Tokens.Dequeue();

            return new NameSyntax
            {
                Value = token.Content,
                Span = token.Span
            };
        }

        private static ListSyntax<ArgumentSyntax> ParseArguments(ParserContext context)
        {
            var token = context.Tokens.Dequeue();
            var span = token!.Span;
            var last = span.End;
            var list = new ListSyntax<ArgumentSyntax>();

            SkipWhitespaceTokens(context);

            if (context.Tokens.Peek()?.Type == TokenType.ParenthesesClose)
            {
                context.Tokens.Dequeue();
                list.Span = span;
                return list;
            }

            while (token != null)
            {
                if (token.Type == TokenType.ParenthesesClose)
                {
                    context.Tokens.Dequeue();
                    break;
                }

                list.Add(ParseArgument(context));
                token = context.Tokens.Peek();
            }

            for (int i = 0; i < list.Children.Count; i++)
            {
                list.Children[i].Position = i;
            }

            if (list.Children.Count > 0)
            {
                last = list.Children[^1].Span.End;
            }

            list.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            return list;
        } 

        private static ArgumentSyntax ParseArgument(ParserContext context)
        {
            var token = context.Tokens.Peek();
            var span = token!.Span;
            var last = span.End;
            var list = new ListSyntax<ListSyntax>();

            SkipWhitespaceTokens(context);

            var nextToken = context.Tokens.Peek(1);
            NameSyntax? name = null;

            if (nextToken?.Type == TokenType.Colon)
            {
                name = ParseName(context);
                // Remove colon token here
                context.Tokens.Dequeue(2);
                token = context.Tokens.Peek();
            }

            SkipWhitespaceTokens(context);

            var argument = new ArgumentSyntax
            {
                Name = name
            };

            while (token != null)
            {
                if (token.Type == TokenType.ParenthesesClose)
                {
                    break;
                }
                else if (token.Type == TokenType.Semicolon)
                {
                    context.Tokens.Dequeue();
                    break;
                }

                list.Add(ParseBlock(context, true));

                last = token.Index + token.Length;
                token = context.Tokens.Peek();
            }

            argument.Content = list;
            argument.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            return argument;
        }

        private static void SkipWhitespaceTokens(ParserContext context)
        {
            context.Tokens.SkipWhile(e => e.Type == TokenType.Newline || e.Type == TokenType.Whitespace);
        }

        private static ListSyntax ParseExplicitBlock(ParserContext context)
        {
            var token = context.Tokens.Dequeue();
            var span = token!.Span;
            var last = span.End;
            var list = new ListSyntax();

            SkipWhitespaceTokens(context);

            if (context.Tokens.Peek()?.Type == TokenType.CurlyClose)
            {
                context.Tokens.Dequeue();
                list.Span = span;
                return list;
            }

            while (token != null)
            {
                if (token.Type == TokenType.CurlyClose)
                {
                    context.Tokens.Dequeue();
                    break;
                }

                list.Add(ParseBlock(context));
                token = context.Tokens.Peek();
            }

            if (list.Children.Count > 0)
            {
                last = list.Children[^1].Span.End;
            }

            list.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            return list;
        }

        public static ParserResult Parse(IEnumerable<Token> tokens)
        {
            return Parse(tokens, null, null);
        }

        public static ParserResult Parse(IEnumerable<Token> tokens, Resource? resource, Logger? logger)
        {
            return ParseInternal(tokens, resource, logger);
        }

        public static ParserResult ParseFromString(string text)
        {
            return ParseFromString(text, null);
        }

        public static ParserResult ParseFromString(string text, Logger? logger)
        {
            return Parse(Lexer.Tokenize(text), null, logger);
        }

        private static ParserResult ParseInternal(IEnumerable<Token> tokens, Resource? resource, Logger? logger)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            var context = new ParserContext
            {
                Tokens = new TokenQueue(tokens)
            };

            if (context.Tokens.Count == 0)
                return ParserResult.Empty();

            var nodes = new List<SyntaxNode>();

            while (context.Tokens.Count > 0)
            {
                nodes.Add(ParseBlock(context));
            }

            return new ParserResult(nodes, context.Issues);
        }

        private static ListSyntax ParseBlock(ParserContext context, bool inArguments = false)
        {
            var token = context.Tokens.Peek();
            var span = token!.Span;
            var list = new ListSyntax();

            while (token != null)
            {
                switch (token.Type)
                {
                    case TokenType.Whitespace:
                    case TokenType.Text:
                        list.Add(ParseText(context, inArguments));
                        break;
                    case TokenType.Percent:
                        list.Add(ParsePercent(context));
                        break;
                    case TokenType.DoubleSlash:
                        list.Add(ParseSingleLineComment(context));
                        break;
                    case TokenType.At:
                        list.Add(ParseCommand(context));
                        break;
                    //case TokenType.Quotation:
                    //    elements.Add(ParseQuotation(context));
                    // Do nothing and end block
                    case TokenType.CurlyClose:
                    case TokenType.Newline:
                    default:
                        goto end;
                }
                token = context.Tokens.Peek();
            }

            
            end:
            var lastSpan = list.Children[^1].Span;
            list.Span = new TextSpan(span.Start, lastSpan.End - span.Start, span.Line);

            context.Tokens.SkipWhile(e => e.Type == TokenType.Newline);

            return list;
        }

        private static void IssuesChanged(Logger logger, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (ParserIssue issue in e.NewItems.OfType<ParserIssue>().Where(e => e != null))
                {
                    logger.Log((LogLevel)(int)issue.Type, issue.Message);
                }
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
            SetContent(element);

            foreach (var child in element.Children.ToArray())
            {
                BuildContent(child);
            }

            if (element.Type == ElementType.Text && string.IsNullOrEmpty(element.Content))
            {
                element.Parent?.Children.Remove(element);
            }

            var children = element.Children.ToArray();

            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                if (child.Type == ElementType.Command && i < children.Length - 1)
                {
                    var next = children[i + 1];
                    if (next.Type == ElementType.ExplicitBlock)
                    {
                        child.Type = ElementType.Environment;
                        next.Detach();
                        next.Parent = child;
                        child.Children.AddLast(next);
                        // Skip the next element
                        i++;
                    }
                }
            }
        }

        private static void SetContent(Element element)
        {
            if (element.StringBuilder != null)
            {
                var text = element.StringBuilder.ToString();

                if (element.Type == ElementType.ObjectField)
                {
                    text = text.Trim(out int advance);
                    element.Index += advance;
                }
                else if (element.Type == ElementType.Text)
                {
                    element.Siblings(out var previous, out var next);

                    if (previous == null || CutOffType(previous.Type))
                    {
                        text = text.TrimStart(out int advance);
                        element.Index += advance;
                    }
                    if (next == null || CutOffType(next.Type))
                    {
                        text = text.TrimEnd();
                    }

                    if (previous != null && previous.Type == ElementType.Command 
                        && next != null && next.Type == ElementType.ExplicitBlock 
                        && string.IsNullOrWhiteSpace(text))
                    {
                        element.Detach();
                        return;
                    }

                    if (previous == null && next == null && text == "null")
                    {
                        element.Type = ElementType.Null;
                    }
                }
                element.Content = text;
                element.StringBuilder = null;
                element.Length = element.Content.Length;

                if (element.Type == ElementType.Quotation)
                {
                    element.Length += 2;
                }
            }
        }

        private static bool CutOffType(ElementType type)
        {
            return !(type == ElementType.Text || type == ElementType.Quotation || type == ElementType.Command); 
        }

        private static Element AppendText(ParserContext context, Element previous, Element parent, Token token)
        {
            var textItem = previous.Type == ElementType.Text ? previous : new Element(parent, ElementType.Text, token.Index, context.Line);

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


    }
}
