using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Scriber.Maths
{
    public class MathToken
    {
        public List<MathToken> Children { get; } = new List<MathToken>();
        public MathTokenType Type { get; set; }
        public string Content { get; set; } = "0";

        public double ApplyOperation(double a, double b)
        {
            if (Type == MathTokenType.Operator)
            {
                return Content switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    _ => throw new InvalidOperationException()
                };
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public double GetContent()
        {
            if (Type == MathTokenType.Constant)
            {
                return double.Parse(Content, CultureInfo.InvariantCulture);
            }
            else if (Type == MathTokenType.Container)
            {
                if (Children.Count == 0)
                {
                    throw new InvalidOperationException();
                }
                else if (Children.Count == 1)
                {
                    var child = Children[0];
                    if (child.Type == MathTokenType.Constant)
                    {
                        return child.GetContent();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                else if (Children.Count % 2 == 1)
                {
                    // In order not to manipulate the state of the token
                    // we just copy the children of the current token.
                    var childrenCopy = new List<MathToken>(Children);
                    for (int i = 0; i < childrenCopy.Count - 1; i += 2)
                    {
                        var a = childrenCopy[i].GetContent();
                        var op = childrenCopy[i + 1];
                        var b = childrenCopy[i + 2].GetContent();

                        if (op.Content == "*" || op.Content == "/")
                        {
                            childrenCopy[i] = new MathToken() {
                                Content = op.ApplyOperation(a, b).ToString(CultureInfo.InvariantCulture),
                                Type = MathTokenType.Constant
                            };
                            childrenCopy.RemoveAt(i + 1);
                            childrenCopy.RemoveAt(i + 1);
                        }
                    }

                    var item = childrenCopy[0].GetContent();
                    for (int i = 1; i < childrenCopy.Count; i += 2)
                    {
                        var op = childrenCopy[i];
                        var b = childrenCopy[i + 1].GetContent();

                        item = op.ApplyOperation(item, b);
                    }

                    return item;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public override string ToString()
        {
            return ToStringInternal(false);
        }

        private string ToStringInternal(bool addParenthesis)
        {
            if (Type == MathTokenType.Constant)
            {
                return Content;
            }
            else if (Type == MathTokenType.Operator)
            {
                return " " + Content + " ";
            }
            else
            {
                var sb = new StringBuilder();
                if (addParenthesis)
                    sb.Append('(');
                foreach (var token in Children)
                {
                    sb.Append(token.ToStringInternal(true));
                }
                if (addParenthesis)
                    sb.Append(')');
                return sb.ToString();
            }
        }
    }

    public enum MathTokenType
    {
        Container,
        Constant,
        Operator,
    }
}
