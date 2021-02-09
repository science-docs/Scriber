using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Layout.Styling
{
    public static class StyleSelectorParser
    {
        public static IStyleSelector Parse(ReadOnlySpan<char> input)
        {
            var tokens = Tokenize(input);
            return Parse(tokens);
        }

        public static Queue<string> Tokenize(ReadOnlySpan<char> input)
        {
            var sb = new StringBuilder();
            var tokens = new Queue<string>();

            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                bool special;
                switch (c)
                {
                    case ':':
                    case ',':
                    case '~':
                    case '(':
                    case ')':
                    case '+':
                    case '.':
                    case '>':
                    case '*':
                        special = true;
                        break;
                    default:
                        special = false;
                        break;
                }

                if (char.IsWhiteSpace(c) || special)
                {
                    if (sb.Length > 0)
                    {
                        tokens.Enqueue(sb.ToString());
                        sb.Clear();
                    }
                    tokens.Enqueue(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
            {
                tokens.Enqueue(sb.ToString());
            }

            return tokens;
        }

        public static IStyleSelector Parse(Queue<string> tokens)
        {
            PopSpace(tokens);
            var left = ParseSimple(tokens);

            if (tokens.TryPeek(out var token) && token == ":")
            {
                var right = Parse(tokens);
                return new ComplexStyleSelector(left, right, StyleOperator.And);
            }

            PopSpace(tokens);
            if (tokens.Count > 0)
            {
                if (tokens.TryPeek(out token))
                {
                    if (token == ">" || token == "," || token == "~" || token == "+")
                    {
                        tokens.Dequeue();
                    }

                    var right = Parse(tokens);
                    ComplexStyleSelector complex;
                    if (token == ">")
                    {
                        complex = new ComplexStyleSelector(left, right, StyleOperator.Child);
                    }
                    else if (token == ",")
                    {
                        complex = new ComplexStyleSelector(left, right, StyleOperator.Or);
                    }
                    else if (token == "~")
                    {
                        complex = new ComplexStyleSelector(left, right, StyleOperator.Previous);
                    }
                    else if (token == "+")
                    {
                        complex = new ComplexStyleSelector(left, right, StyleOperator.Next);
                    }
                    else
                    {
                        complex = new ComplexStyleSelector(left, right, StyleOperator.Descendant);
                    }
                    return complex;
                }
            }
            return left;
        }

        private static void PopSpace(Queue<string> tokens)
        {
            while (tokens.TryPeek(out var token) && token.Length == 1 && char.IsWhiteSpace(token[0]))
            {
                tokens.Dequeue();
            }
        }

        private static IStyleSelector ParseSimple(Queue<string> tokens)
        {
            var token = tokens.Dequeue();
            if (token == ":")
            {
                var pseudoClass = tokens.Dequeue();

                if (tokens.TryPeek(out token) && token == "(")
                {
                    pseudoClass += token;
                    tokens.Dequeue();
                    int count = 1;
                    while (tokens.TryDequeue(out token))
                    {
                        pseudoClass += token;
                        if (token == "(")
                        {
                            count++;
                        }
                        else if (token == ")")
                        {
                            count--;

                            if (count == 0)
                            {
                                break;
                            }
                        }
                    }
                }

                return new PseudoClassSelector(pseudoClass);
            }
            else if (token == ".")
            {
                var classes = new List<string>();
                var isDot = true;
                while (isDot && tokens.TryDequeue(out token))
                {
                    classes.Add(token);
                    isDot = tokens.TryPeek(out token) && token == ".";
                    if (isDot)
                    {
                        tokens.Dequeue();
                    }
                }
                return new ClassStyleSelector(classes);
            }
            else if (token == "*")
            {
                return AllStyleSelector.Singleton;
            }
            else
            {
                return new TagStyleSelector(token);
            }
        }
    }
}
