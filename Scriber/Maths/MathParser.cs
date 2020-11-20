using System;
using System.Text;

namespace Scriber.Maths
{
    public static class MathParser
    {
        public static bool TryEvaluate(string expression, out MathResult result)
        {
            try
            {
                result = Evaluate(expression);
                return true;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                result = new MathResult(double.NaN, expression);
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        public static MathResult Evaluate(string expression)
        {
            return new MathResult(Parse(expression).GetContent(), expression);
        }

        public static MathToken Parse(string expression)
        {
            var i = 0;
            return Parse(expression, true, ref i);
        }

        private static MathToken Parse(string expression, bool useSign, ref int index)
        {
            var container = new MathToken();
            var sb = new StringBuilder();
            var cur = new MathToken();

            for (; index < expression.Length; index++)
            {
                var c = expression[index];
                if (char.IsDigit(c))
                {
                    cur.Type = MathTokenType.Constant;
                    sb.Append(c);
                    useSign = false;
                }
                else if (c == '.')
                {
                    cur.Type = MathTokenType.Constant;
                    sb.Append(c);
                    useSign = false;
                }
                else if (useSign && (c == '-' || c == '+'))
                {
                    sb.Append(c);
                    useSign = false;
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    if (sb.Length == 0)
                    {
                        throw new InvalidOperationException();
                    }

                    cur.Content = sb.ToString();
                    container.Children.Add(cur);
                    container.Children.Add(new MathToken
                    {
                        Content = c.ToString(),
                        Type = MathTokenType.Operator
                    });
                    sb.Clear();
                    cur = new MathToken();
                    useSign = true;
                }
                else if (c == '(')
                {
                    index++;
                    sb.Clear();
                    container.Children.Add(Parse(expression, true, ref index));
                }
                else if (c == ')')
                {
                    cur.Content = sb.ToString();
                    sb.Clear();
                    container.Children.Add(cur);
                    return container;
                }
                else if (!char.IsWhiteSpace(c))
                {
                    throw new Exception($"Invalid character '{c}' found in math expression.");
                }
            }

            if (sb.Length > 0)
            {
                cur.Content = sb.ToString();
                container.Children.Add(cur);
            }

            return container;
        }
    }
}
