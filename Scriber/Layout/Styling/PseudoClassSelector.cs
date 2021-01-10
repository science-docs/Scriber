using Scriber.Layout.Document;
using Scriber.Layout.Styling.Classes;
using System;

namespace Scriber.Layout.Styling
{
    public class PseudoClassSelector : ClassStyleSelector
    {
        public override int Specificity => 3;

        public string PseudoClassText { get; }
        public PseudoClass PseudoClass { get; }

        public PseudoClassSelector(string className, string pseudoClass) : base(className)
        {
            PseudoClassText = pseudoClass ?? throw new ArgumentNullException(nameof(pseudoClass));
            PseudoClass = PseudoClass.FromString(pseudoClass);
        }

        public override bool Matches(AbstractElement element)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(Class))
            {
                result = base.Matches(element);
            }

            if (result)
            {
                return PseudoClass.Matches(element);
            }

            return false;
        }

        public override string ToString()
        {
            return $"{Class}:{PseudoClass}";
        }
    }
}
