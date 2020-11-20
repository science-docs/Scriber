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
                    line++;
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

                if (cur != null)
                {
                    if (IsSpecialCharacter(c))
                    {
                        cur.Content = sb.ToString();
                        sb.Clear();
                        // do not return empty text passages
                        tokens.Add(cur);
                        cur = null;
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                // as cur can become null during the previous branch, we have to check it again
                if (cur == null)
                {
                    if (IsSpecialCharacter(c))
                    {
                        var type = GetTokenType(c);
                        if (IsSingleToken(type))
                        {
                            tokens.Add(new Token(type, i, line, c.ToString()));
                        }
                        else
                        {
                            cur = new Token(GetTokenType(c), i, line);
                            sb.Append(c);
                        }
                    }
                    else
                    {
                        cur = new Token(TokenType.Text, i, line);
                        sb.Append(c);
                    }

                }
            }
            
            if (cur != null)
            {
                cur.Content = sb.ToString();
                tokens.Add(cur);
            }

            return tokens;
        }

        private static bool IsSpecialCharacter(char c)
        {
            return c == '\\' || c == ',' || c == ':' || c == '"' || c == '@' || c == '%' || c == '\n' || c == '$' || c == '&' || c == '~' || c == '_' || c == '(' || c == ')' || c == '{' || c == '}' || c == '^' || c == '[' || c == ']';
        }

        private static bool IsSingleToken(TokenType token)
        {
            return token switch
            {
                TokenType.At => false,
                TokenType.Backslash => false,
                TokenType.Text => false,
                TokenType.Underscore => false,
                TokenType.Caret => false,
                _ => true
            };
        }

        private static TokenType GetTokenType(char c)
        {
            return c switch
            {
                '@' => TokenType.At,
                '"' => TokenType.Quotation,
                '%' => TokenType.Percent,
                '\n' => TokenType.Newline,
                '$' => TokenType.Currency,
                '&' => TokenType.Ampersand,
                '~' => TokenType.Tilde,
                '_' => TokenType.Underscore,
                '(' => TokenType.ParenthesesOpen,
                ')' => TokenType.ParenthesesClose,
                '{' => TokenType.CurlyOpen,
                '}' => TokenType.CurlyClose,
                '[' => TokenType.BracketOpen,
                ']' => TokenType.BracketClose,
                ':' => TokenType.Colon,
                ',' => TokenType.Comma,
                '^' => TokenType.Caret,
                _ => TokenType.Text
            };
        }
    }
}
