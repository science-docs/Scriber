using Scriber.Text;

namespace Scriber.Variables
{
    public static class FontVariables
    {
        public static DocumentLocal<Font> Font { get; } = new DocumentLocal<Font>(() => Text.Font.Default);
        public static DocumentLocal<double> FontSize { get; } = new DocumentLocal<double>(11);
        public static DocumentLocal<double?> FootnoteSize { get; } = new DocumentLocal<double?>((double?)null); 
    }
}
