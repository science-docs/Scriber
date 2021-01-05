using Scriber.Layout.Document;
using System;

namespace Scriber.Layout.Styling
{
    public class ClassStyleSelector : StyleSelector
    {
        public override int Specificity => 2;

        public string Class { get; }

        public ClassStyleSelector(string className)
        {
            Class = className ?? throw new ArgumentNullException(nameof(className));
        }

        public override bool Matches(AbstractElement element)
        {
            foreach (var className in element.Classes)
            {
                if (className.Equals(Class, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
