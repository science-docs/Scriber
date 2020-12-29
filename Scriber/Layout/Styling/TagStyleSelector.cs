using Scriber.Layout.Document;
using System;

namespace Scriber.Layout.Styling
{
    public class TagStyleSelector : StyleSelector
    {
        public string Tag { get; set; }

        public TagStyleSelector(string tag)
        {
            Tag = tag ?? throw new ArgumentNullException(nameof(tag));
        }

        public override bool Matches(AbstractElement element)
        {
            return Tag.Equals(element.Tag, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
