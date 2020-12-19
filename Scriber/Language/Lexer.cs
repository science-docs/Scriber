using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Language
{
    public static class Lexer
    {


        public static IEnumerable<Token> Tokenize(ReadOnlySpan<char> span)
        {
            int line = 0;
            bool ignore = false;
            bool inStringLiteral = false;
            char c;
            Token? cur = null;
            StringBuilder sb = new StringBuilder();
            var tokens = new List<Token>(span.Length / 50);

            for (int i = 0; i < span.Length; i++)
            {
                c = span[i];

                if (c == '\r')
                {
                    continue;
                }

                if (c == '\n')
                {
                    EndCurrentToken(tokens, sb, ref cur);
                    tokens.Add(new Token(TokenType.Newline, i, line++, c.ToString()));
                    continue;
                }
                else
                {
                    if (ignore)
                    {
                        if (!IsSpecialCharacter(c))
                        {
                            sb.Append('\\');
                        }

                        sb.Append(c);
                        ignore = false;
                        continue;
                    }
                    else if (c == '\\')
                    {
                        ignore = true;
                        continue;
                    }
                }

                ignore = false;

                int index = i;
                if (IsNext(span, ref i, "/*"))
                {
                    EndCurrentToken(tokens, sb, ref cur);
                    tokens.Add(new Token(TokenType.SlashAsterisk, index, line, "/*"));
                }
                else if (IsNext(span, ref i, "//"))
                {
                    EndCurrentToken(tokens, sb, ref cur);
                    tokens.Add(new Token(TokenType.DoubleSlash, index, line, "//"));
                }
                else if (IsNext(span, ref i, "*/"))
                {
                    EndCurrentToken(tokens, sb, ref cur);
                    tokens.Add(new Token(TokenType.AsteriskSlash, index, line, "*/"));
                }
                else if (IsNext(span, ref i, "@\""))
                {
                    inStringLiteral = true;
                    EndCurrentToken(tokens, sb, ref cur);
                    tokens.Add(new Token(TokenType.QuotedRaw, index, line, "@\""));
                }
                else if (inStringLiteral && IsNext(span, ref i, "{{"))
                {
                    EndCurrentToken(tokens, sb, ref cur);
                    tokens.Add(new Token(TokenType.DoubleCurlyOpen, index, line, "{{"));
                }
                else if (inStringLiteral && IsNext(span, ref i, "}}"))
                {
                    EndCurrentToken(tokens, sb, ref cur);
                    tokens.Add(new Token(TokenType.DoubleCurlyClose, index, line, "}}"));
                }
                else if (IsSpecialCharacter(c))
                {
                    EndCurrentToken(tokens, sb, ref cur);
                    tokens.Add(new Token(GetTokenType(c), i, line, c.ToString()));

                    if (c == '"')
                    {
                        inStringLiteral = false;
                    }
                }
                else
                {
                    if (cur == null)
                    {
                        cur = new Token(i, line);
                    }
                    sb.Append(c);
                }
            }
            
            if (cur != null)
            {
                cur.Content = sb.ToString();
                tokens.Add(cur);
            }

            return tokens;
        }

        private static void EndCurrentToken(List<Token> tokens, StringBuilder sb, ref Token? token)
        {
            if (token != null && sb.Length > 0)
            {
                token.Content = sb.ToString();
                sb.Clear();
                tokens.Add(token);
                token = null;
            }
        }

        private static bool IsSpecialCharacter(char c)
        {
            return char.IsWhiteSpace(c) || c == ';' || c == ':' || c == '"' || c == '@' || c == '%' || c == '\n' || c == '$' || c == '&' || c == '~' || c == '_' || c == '(' || c == ')' || c == '{' || c == '}' || c == '^' || c == '[' || c == ']';
        }

        private static bool IsNext(ReadOnlySpan<char> span, ref int index, string value)
        {
            if (value.Length + index > span.Length)
            {
                return false;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (span[i + index] != value[i])
                {
                    return false;
                }
            }


            index += value.Length - 1;
            return true;
        }

        private static TokenType GetTokenType(char c)
        {
            if (char.IsWhiteSpace(c))
            {
                return TokenType.Whitespace;
            }

            return c switch
            {
                '@' => TokenType.At,
                '"' => TokenType.Quotation,
                '%' => TokenType.Percent,
                '\n' => TokenType.Newline,
                '$' => TokenType.Currency,
                '~' => TokenType.Tilde,
                '_' => TokenType.Underscore,
                '(' => TokenType.ParenthesesOpen,
                ')' => TokenType.ParenthesesClose,
                '{' => TokenType.CurlyOpen,
                '}' => TokenType.CurlyClose,
                '[' => TokenType.BracketOpen,
                ']' => TokenType.BracketClose,
                ':' => TokenType.Colon,
                ';' => TokenType.Semicolon,
                '^' => TokenType.Caret,
                _ => TokenType.Text
            };
        }
    }
}
