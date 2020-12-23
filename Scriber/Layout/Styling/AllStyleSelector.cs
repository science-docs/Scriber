using Scriber.Layout.Document;

namespace Scriber.Layout.Styling
{
    public class AllStyleSelector : StyleSelector
    {
        public override bool Matches(AbstractElement element)
        {
            return true;
        }
    }
}
