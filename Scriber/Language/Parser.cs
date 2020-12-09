using Scriber.Language.Syntax;
using Scriber.Logging;
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

        private static SyntaxNode ParseText(ParserContext context, params TokenType[] stop)
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
                else if (stop.Contains(token.Type) || NeverTextToken(token.Type)) // && (token.Type == TokenType.ParenthesesClose || token.Type == TokenType.Semicolon || token.Type == TokenType.BracketClose))
                {
                    break;
                }
                else //if (IsTextToken(token.Type))
                {
                    context.Tokens.Dequeue();
                    whitespace = false;
                    sb.Append(token.Content);
                }
                //else
                //{
                //    break;
                //}

                last = token.Index + token.Length;
                token = context.Tokens.Peek();
            }

            return new TextSyntax
            {
                Span = new TextSpan(start, last - start, line),
                Text = sb.ToString()
            };
        }

        private static bool NeverTextToken(TokenType type)
        {
            return type == TokenType.Percent || type == TokenType.At || type == TokenType.DoubleSlash || type == TokenType.SlashAsterisk || type == TokenType.AsteriskSlash || type == TokenType.Quotation;
        }

        private static SyntaxNode ParseSinglelineComment(ParserContext context)
        {
            var token = context.Tokens.Dequeue();
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

        private static SyntaxNode ParseMultilineComment(ParserContext context)
        {
            var token = context.Tokens.Dequeue();
            int start = token!.Index;
            int line = token!.Line;
            var lastSpan = token!.Span;

            var sb = new StringBuilder();

            while (token != null)
            {
                sb.Append(token.Content);
                lastSpan = token.Span;
                if (token.Type == TokenType.AsteriskSlash)
                {
                    context.Tokens.Dequeue();
                    break;
                }
                token = context.Tokens.Dequeue();
            }

            var comment = new CommentSyntax
            {
                Span = new TextSpan(start, lastSpan.End - start, line),
                Multiline = true,
                Comment = sb.ToString()
            };

            if (token == null)
            {
                var end = lastSpan.End;
                context.Issues.Add(new TextSpan(end - 1, 1, lastSpan.Line), ParserIssueType.Error, "Expected end of multiline comment */");
            }

            return comment;
        }

        private static SyntaxNode ParseQuotation(ParserContext context)
        {
            var token = context.Tokens.Dequeue();
            int start = token!.Index;
            int line = token!.Line;
            int last = start + token!.Length;

            var sb = new StringBuilder();

            token = context.Tokens.Dequeue();
            while (token != null)
            {
                if (token.Type == TokenType.Quotation)
                {
                    last = token.Index + token.Length;
                    break;
                }
                else
                {
                    sb.Append(token.Content);
                }
                last = token.Index + token.Length;
                token = context.Tokens.Dequeue();
            }

            return new StringLiteralSyntax
            {
                Span = new TextSpan(start, last - start, line),
                Content = sb.ToString()
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
                command.Environment = ParseExplicitBlock(context);
            }

            if (command.Arity > 0)
            {
                command.Span = new TextSpan(start, command.Arguments[^1].Span.End - start, line);
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
                return new NameSyntax();
            }

            if (token.Type != TokenType.Text)
            {
                context.Issues.Add(token, ParserIssueType.Error, "Expected a name.");
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
                else if (token.Type == TokenType.Semicolon)
                {
                    context.Tokens.Dequeue();
                }

                list.Add(ParseArgument(context));
                token = context.Tokens.Peek();
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Position = i;
            }

            if (list.Count > 0)
            {
                last = list[^1].Span.End;
            }

            list.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            return list;
        } 

        private static ArgumentSyntax ParseArgument(ParserContext context)
        {
            var token = context.Tokens.Peek();
            var span = token!.Span;
            var last = span.End;

            SkipWhitespaceTokens(context);

            var nextToken = context.Tokens.Peek(1);
            NameSyntax? name = null;

            if (nextToken?.Type == TokenType.Colon)
            {
                name = ParseName(context);
                // Remove colon token here
                context.Tokens.Dequeue(2);
            }

            SkipWhitespaceTokens(context);

            var argument = new ArgumentSyntax
            {
                Name = name
            };

            token = context.Tokens.Peek();
            if (token != null && token.Type != TokenType.Semicolon && token.Type != TokenType.ParenthesesClose)
            {
                argument.Content = ParseBlock(context, TokenType.Semicolon, TokenType.ParenthesesClose);
            }
            else
            {
                argument.Content = new ListSyntax();
            }

            token = context.Tokens.Peek();

            if (token != null)
            {
                last = token.Span.End;
            }

            if (token == null || (token.Type != TokenType.ParenthesesClose && token.Type != TokenType.Semicolon))
            {
                // Add issue here
            }

            CleanText(argument.Content);


            argument.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            // Some elements (like objects or arrays) have to be the only syntax node in the argument.
            // Therefore, we create appropriate issue messages here.
            if (argument.Content.Count > 1)
            {
                if (argument.Content.Any(e => e is ObjectSyntax))
                {
                    context.Issues.Add(argument, ParserIssueType.Error, "Objects have to be the only element in an argument.");
                }
                if (argument.Content.Any(e => e is ArraySyntax))
                {
                    context.Issues.Add(argument, ParserIssueType.Error, "Arrays have to be the only element in an argument.");
                }
            }

            return argument;
        }

        private static void CleanText(ListSyntax list)
        {
            if (list.Count > 0 && list[^1] is TextSyntax textSyntax)
            {
                textSyntax.Text = textSyntax.Text.TrimEnd();
            }
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

            if (list.Count > 0)
            {
                last = list[^1].Span.End;
            }

            list.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            return list;
        }

        private static ObjectSyntax ParseObject(ParserContext context)
        {
            var token = context.Tokens.Dequeue();
            var span = token!.Span;
            var last = span.End;
            var obj = new ObjectSyntax();
            var list = new ListSyntax<FieldSyntax>();
            obj.Fields = list;

            SkipWhitespaceTokens(context);

            if (context.Tokens.Peek()?.Type == TokenType.CurlyClose)
            {
                context.Tokens.Dequeue();
                list.Span = span;
                return obj;
            }

            while (token != null)
            {
                if (token.Type == TokenType.CurlyClose)
                {
                    context.Tokens.Dequeue();
                    break;
                }

                var field = ParseField(context);
                if (field != null)
                {
                    list.Add(field);
                }
                token = context.Tokens.Peek();
            }

            if (list.Count > 0)
            {
                last = list[^1].Span.End;
            }

            list.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            return obj;
        }

        private static FieldSyntax? ParseField(ParserContext context)
        {
            var token = context.Tokens.Peek();
            var span = token!.Span;
            var last = span.End;

            SkipWhitespaceTokens(context);

            var nextToken = context.Tokens.Peek(1);
            NameSyntax? name;
            if (nextToken?.Type == TokenType.Colon)
            {
                name = ParseName(context);
                // Remove colon token here
                context.Tokens.Dequeue(2);
                token = context.Tokens.Peek();
            }
            else
            {
                // In order to parse the next field correctly, we read the current field to the end.
                ParseBlock(context, TokenType.Semicolon, TokenType.CurlyClose);
                nextToken = context.Tokens.Peek() ?? nextToken;
                if (nextToken != null)
                {
                    last = nextToken.Span.Start;
                }
                span = new TextSpan(span.Start, last - span.Start, span.Line);
                
                context.Issues.Add(span, ParserIssueType.Error, "Field name and : expected.");
                return null;
            }

            SkipWhitespaceTokens(context);

            var argument = new FieldSyntax
            {
                Name = name
            };

            if (token != null)
            {
                argument.Value = ParseBlock(context, TokenType.Semicolon, TokenType.CurlyClose);
            }
            else
            {
                argument.Value = new ListSyntax();
            }

            token = context.Tokens.Peek();

            if (token != null)
            {
                last = token.Span.End;
                if (token.Type == TokenType.Semicolon)
                {
                    context.Tokens.Dequeue();
                }
                else if (token.Type != TokenType.CurlyClose)
                {
                    context.Issues.Add(token, ParserIssueType.Error, "Expected the end of a field via ; or }.");
                }
            }
            else
            {
                context.Issues.Add(argument, ParserIssueType.Error, "Expected the end of a field via ; or }.");
            }

            CleanText(argument.Value);

            argument.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            return argument;
        }

        private static ArraySyntax ParseArray(ParserContext context)
        {
            var token = context.Tokens.Dequeue();
            var span = token!.Span;
            var last = span.End;
            var array = new ArraySyntax();

            while (token != null)
            {
                if (token.Type == TokenType.BracketClose)
                {
                    context.Tokens.Dequeue();
                    break;
                }
                if (token.Type == TokenType.Semicolon)
                {
                    context.Tokens.Dequeue();
                }

                array.Add(ParseArrayElement(context));
                token = context.Tokens.Peek();
            }

            SkipWhitespaceTokens(context);

            if (array.Count > 0)
            {
                last = array[^1].Span.End;
            }

            array.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            return array;
        }

        private static ListSyntax ParseArrayElement(ParserContext context)
        {
            SkipWhitespaceTokens(context);
            var token = context.Tokens.Peek();

            if (token == null)
            {
                return new ListSyntax();
            }

            var span = token!.Span;
            var last = span.End;

            var list = ParseBlock(context, TokenType.Semicolon, TokenType.BracketClose);

            token = context.Tokens.Peek();
            
            list.Span = new TextSpan(span.Start, last - span.Start, span.Line);

            CleanText(list);

            if (token == null || (token.Type != TokenType.BracketClose && token.Type != TokenType.Semicolon))
            {
                context.Issues.Add(list, ParserIssueType.Error, "Expected the end of an array element via ; or ].");
            }

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
                Tokens = new TokenQueue(tokens),
                Resource = resource
            };

            if (context.Tokens.Count == 0)
                return ParserResult.Empty();

            if (logger != null)
            {
                context.Issues.CollectionChanged += (_, e) => IssuesChanged(logger, e);
            }

            var nodes = new List<SyntaxNode>();

            while (context.Tokens.Count > 0)
            {
                nodes.Add(ParseBlock(context));
            }

            return new ParserResult(nodes, context.Issues);
        }

        private static ListSyntax ParseBlock(ParserContext context, params TokenType[] stop)
        {
            var token = context.Tokens.Peek();
            var startToken = token;
            var span = token!.Span;
            var list = new ListSyntax();

            while (token != null)
            {
                switch (token.Type)
                {
                    case TokenType.Whitespace:
                    case TokenType.Text:
                        list.Add(ParseText(context, stop));
                        break;
                    case TokenType.Percent:
                        list.Add(ParsePercent(context));
                        break;
                    case TokenType.DoubleSlash:
                        list.Add(ParseSinglelineComment(context));
                        break;
                    case TokenType.SlashAsterisk:
                        list.Add(ParseMultilineComment(context));
                        break;
                    case TokenType.At:
                        list.Add(ParseCommand(context));
                        break;
                    case TokenType.Quotation:
                        list.Add(ParseQuotation(context));
                        break;
                    case TokenType.CurlyOpen:
                        if (stop.Contains(TokenType.Semicolon))
                        {
                            list.Add(ParseObject(context));
                        }
                        else
                        {
                            list.Add(ParseText(context, stop));
                        }
                        break;
                    case TokenType.BracketOpen:
                        if (stop.Contains(TokenType.Semicolon))
                        {
                            list.Add(ParseArray(context));
                        }
                        else
                        {
                            list.Add(ParseText(context, stop));
                        }
                        break;
                    case TokenType.Newline:
                        SkipWhitespaceTokens(context);
                        goto end;
                    //if (context.Tokens.Peek(2)?.Type == TokenType.Newline)
                    //{

                    //}
                    //else
                    //{
                    //    list.Add(ParseText(context, stop));
                    //}
                    //break;
                    default:
                        if (!stop.Contains(token.Type))
                        {
                            list.Add(ParseText(context, stop));
                            break;
                        }
                        else
                        {
                            goto end;
                        }
                }
                token = context.Tokens.Peek();
            }

            
            end:

            if (list.Count > 0)
            {
                var lastSpan = list[^1].Span;
                list.Span = new TextSpan(span.Start, lastSpan.End - span.Start, span.Line);
            }
            else
            {
                list.Span = span;
            }
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
    }
}
