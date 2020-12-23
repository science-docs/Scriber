using Scriber.Layout.Document;

namespace Scriber.Layout.Styling
{
    public class TagStyleSelector : StyleSelector
    {
        public string Tag { get; set; }

        public TagStyleSelector(string tag)
        {
            Tag = tag;
        }

        public override bool Matches(AbstractElement element)
        {
            return element.Tag == Tag;
        }
    }
}
