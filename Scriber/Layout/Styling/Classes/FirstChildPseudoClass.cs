using Scriber.Layout.Document;
using System.Linq;

namespace Scriber.Layout.Styling.Classes
{
    public class FirstChildPseudoClass : PseudoClass
    {
        public override bool Matches(AbstractElement element)
        {
            var parent = element.Parent;
            if (parent is null)
                return false;

            var parentNodes = parent.ChildElements();
            return parentNodes.FirstOrDefault() == element;
        }

        public override string ToString()
        {
            return "first-child";
        }
    }
}
