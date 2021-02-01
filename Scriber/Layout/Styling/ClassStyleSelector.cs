using Scriber.Layout.Document;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Layout.Styling
{
    public class ClassStyleSelector : StyleSelector
    {
        public override int Specificity => 2;

        public IReadOnlyCollection<string> Classes { get; }

        public ClassStyleSelector(string selectorText)
        {
            if (selectorText is null)
            {
                throw new ArgumentNullException(nameof(selectorText));
            }

            Classes = selectorText.Split('.', StringSplitOptions.RemoveEmptyEntries);
        }

        public ClassStyleSelector(IEnumerable<string> classes)
        {
            if (classes is null)
            {
                throw new ArgumentNullException(nameof(classes));
            }

            Classes = classes.ToArray();
        }

        public override bool Matches(AbstractElement element)
        {
            return Classes.All(e => element.Classes.Contains(e));
        }

        public override string ToString()
        {
            return "." + string.Join('.', Classes);
        }
    }
}
