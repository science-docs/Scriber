using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tex.Net.Language
{
    public static class Lexer
    {
        public static IEnumerable<Token> Tokenize(string text)
        {
            using var sr = new StringReader(text);
            return Tokenize(sr).ToArray();
        }

        public static IEnumerable<Token> Tokenize(TextReader reader)
        {
            int code;
            int index = 0;
            char c;
            Token cur = null;
            StringBuilder sb = new StringBuilder();

            while ((code = reader.Read()) != -1)
            {
                c = (char)code;

                if (cur != null)
                {
                    if (IsSpecialCharacter(c) || (cur.Type == TokenType.Backslash && char.IsWhiteSpace(c)))
                    {
                        cur.Content = sb.ToString();
                        sb.Clear();
                        yield return cur;
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
                            yield return new Token(type, index) { Content = c.ToString() };
                        }
                        else
                        {
                            cur = new Token(GetTokenType(c), index);
                            sb.Append(c);
                        }
                    }
                    else
                    {
                        cur = new Token(TokenType.Text, index);
                        sb.Append(c);
                    }

                }

                index++;
            }
            
            if (cur != null)
            {
                cur.Content = sb.ToString();
                yield return cur;
            }
        }

        private static bool IsSpecialCharacter(char c)
        {
            return c == '\\' || c == '%' || c == '\n' || c == '$' || c == '&' || c == '~' || c == '_' || c == '{' || c == '}' || c == '^' || c == '[' || c == ']';
        }

        private static bool IsSingleToken(TokenType token)
        {
            return token switch
            {
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
                '\\' => TokenType.Backslash,
                '%' => TokenType.Percent,
                '\n' => TokenType.Newline,
                '$' => TokenType.Currency,
                '&' => TokenType.Ampersand,
                '~' => TokenType.Tilde,
                '_' => TokenType.Underscore,
                '{' => TokenType.CurlyOpen,
                '}' => TokenType.CurlyClose,
                '[' => TokenType.BracketOpen,
                ']' => TokenType.BracketClose,
                '^' => TokenType.Caret,
                _ => TokenType.Text
            };
        }
    }
}
