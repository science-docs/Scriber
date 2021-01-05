using Scriber.Layout.Document;
using System;

namespace Scriber.Layout.Styling.Classes
{
    public class NotPseudoClass : PseudoClass
    {
        public StyleSelector Selector { get; }

        public NotPseudoClass(StyleSelector selector)
        {
            Selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public override bool Matches(AbstractElement element)
        {
            return !Selector.Matches(element);
        }
    }
}
