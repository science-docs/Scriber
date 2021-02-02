using Scriber.Layout.Document;

namespace Scriber.Layout.Styling
{
    public interface IStyleSelector
    {
        Priority Specificity { get; }

        bool Matches(AbstractElement element);
    }
}
