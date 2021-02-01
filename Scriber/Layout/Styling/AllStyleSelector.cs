using Scriber.Layout.Document;

namespace Scriber.Layout.Styling
{
    public class AllStyleSelector : StyleSelector
    {
        public static AllStyleSelector Singleton { get; } = new AllStyleSelector();

        private AllStyleSelector()
        {
        }

        public override int Specificity => 0;

        public override bool Matches(AbstractElement element)
        {
            return true;
        }

        public override string ToString()
        {
            return "*";
        }
    }
}
