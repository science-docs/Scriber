using Scriber.Layout.Document;
using System;

namespace Scriber.Layout.Styling
{
    public class TagStyleSelector : IStyleSelector
    {
        public string Tag { get; set; }

        public Priority Specificity => Priority.OneTag;

        public TagStyleSelector(string tag)
        {
            Tag = tag ?? throw new ArgumentNullException(nameof(tag));
        }

        public bool Matches(AbstractElement element)
        {
            return Tag.Equals(element.Tag, StringComparison.InvariantCultureIgnoreCase);
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}
