using Scriber.Layout.Document;

namespace Scriber.Layout.Styling
{
    public class AllStyleSelector : IStyleSelector
    {
        public static AllStyleSelector Singleton { get; } = new AllStyleSelector();

        public Priority Specificity => Priority.Zero;

        private AllStyleSelector()
        {
        }

        public bool Matches(AbstractElement element)
        {
            return true;
        }

        public override string ToString()
        {
            return "*";
        }
    }
}
