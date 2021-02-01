using Scriber.Layout.Document;
using Scriber.Layout.Styling.Classes;
using System;

namespace Scriber.Layout.Styling
{
    public class PseudoClassSelector : StyleSelector
    {
        public override int Specificity => 3;

        public string PseudoClassText { get; }
        public PseudoClass PseudoClass { get; }

        public PseudoClassSelector(string pseudoClass)
        {
            PseudoClassText = pseudoClass ?? throw new ArgumentNullException(nameof(pseudoClass));
            PseudoClass = PseudoClass.FromString(pseudoClass);
        }

        public override bool Matches(AbstractElement element)
        {
            return PseudoClass.Matches(element);
        }

        public override string ToString()
        {
            return $":{PseudoClass}";
        }
    }
}
