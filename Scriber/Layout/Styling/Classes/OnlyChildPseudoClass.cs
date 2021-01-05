using Scriber.Layout.Document;

namespace Scriber.Layout.Styling.Classes
{
    public class OnlyChildPseudoClass : PseudoClass
    {
        public override bool Matches(AbstractElement element)
        {
            var parent = element.Parent;
            if (parent is null)
                return false;

            var parentNodes = parent.ChildElements();
            using var en = parentNodes.GetEnumerator();
            AbstractElement? result = null;
            if (en.MoveNext())
            {
                result = en.Current;
            }
            if (en.MoveNext())
            {
                result = null;
            }
            return result == element;
        }
    }
}
