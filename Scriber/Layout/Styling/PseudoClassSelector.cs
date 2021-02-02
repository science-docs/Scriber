using Scriber.Layout.Document;
using Scriber.Layout.Styling.Classes;
using System;

namespace Scriber.Layout.Styling
{
    public class PseudoClassSelector : IStyleSelector
    {
        public string PseudoClassText { get; }
        public PseudoClass PseudoClass { get; }

        public Priority Specificity => Priority.OneClass;

        public PseudoClassSelector(string pseudoClass)
        {
            PseudoClassText = pseudoClass ?? throw new ArgumentNullException(nameof(pseudoClass));
            PseudoClass = PseudoClass.FromString(pseudoClass);
        }

        public bool Matches(AbstractElement element)
        {
            return PseudoClass.Matches(element);
        }

        public override string ToString()
        {
            return $":{PseudoClass}";
        }
    }
}
