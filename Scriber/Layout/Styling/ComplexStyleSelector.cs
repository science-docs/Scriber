using Scriber.Layout.Document;
using System.Linq;

namespace Scriber.Layout.Styling
{
    public enum StyleOperator
    {
        Or,
        And,
        Descendant,
        Child,
        Previous,
        Next
    }

    public class ComplexStyleSelector : IStyleSelector
    {
        public IStyleSelector Left { get; }
        public IStyleSelector Right { get; }
        public StyleOperator Operator { get; }

        public Priority Specificity { get; }

        public ComplexStyleSelector(IStyleSelector left, IStyleSelector right, StyleOperator op)
        {
            Left = left;
            Right = right;
            Operator = op;
            Specificity = CalculateComplexPriority();
        }

        private Priority CalculateComplexPriority()
        {
            var left = Left.Specificity;
            var right = Right.Specificity;
            if (Operator == StyleOperator.Or)
            {
                var compared = left.CompareTo(right);
                if (compared > 0)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
            else
            {
                return left + right;
            }
        }

        public bool Matches(AbstractElement element)
        {
            if (Operator == StyleOperator.Or)
            {
                return Left.Matches(element) || Right.Matches(element);
            }
            else if (Operator == StyleOperator.And)
            {
                return Left.Matches(element) && Right.Matches(element);
            }

            var parent = element.Parent;

            if (parent is null || !Right.Matches(element))
            {
                return false;
            }

            if (Operator == StyleOperator.Child)
            {
                return Left.Matches(parent);
            }
            else if (Operator == StyleOperator.Descendant)
            {
                while (parent != null)
                {
                    if (Left.Matches(parent))
                    {
                        return true;
                    }
                    parent = parent.Parent;
                }
                return false;
            }

            var children = parent.ChildElements().ToList();
            var index = children.IndexOf(element);

            if (Operator == StyleOperator.Previous && index > 0)
            {
                return Left.Matches(children[index - 1]);
            }
            else if (Operator == StyleOperator.Next && index < children.Count - 1)
            {
                return Left.Matches(children[index + 1]);
            }
            else
            {
                return false;
            }
        }
    }
}
