using Scriber.Layout.Document;

namespace Scriber.Layout.Styling
{
    public abstract class StyleSelector
    {
        public const int MaxSpecificity = 3;

        public abstract int Specificity { get; }

        public abstract bool Matches(AbstractElement element);

        public static StyleSelector FromString(string path)
        {
            return StyleSelectorParser.Parse(path);
        }
    }
}
