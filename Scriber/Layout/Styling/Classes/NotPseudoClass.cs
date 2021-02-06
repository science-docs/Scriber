using Scriber.Layout.Document;
using System;

namespace Scriber.Layout.Styling.Classes
{
    public class NotPseudoClass : PseudoClass
    {
        public IStyleSelector Selector { get; }

        public NotPseudoClass(IStyleSelector selector)
        {
            Selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public override bool Matches(AbstractElement element)
        {
            return !Selector.Matches(element);
        }

        public override string ToString()
        {
            return $"not({Selector})";
        }
    }
}
