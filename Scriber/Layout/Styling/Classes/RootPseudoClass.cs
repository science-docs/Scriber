using Scriber.Layout.Document;

namespace Scriber.Layout.Styling.Classes
{
    public class RootPseudoClass : PseudoClass
    {
        public override bool Matches(AbstractElement element)
        {
            return element is Scriber.Document;
        }
    }
}
