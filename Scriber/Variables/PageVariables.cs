using Scriber.Layout;

namespace Scriber.Variables
{
    public static class PageVariables
    {
        public static DocumentLocal<Size> BoxSize { get; } = new ComputedDocumentLocal<Size>(doc =>
        {
            var size = doc.Variable(Size);
            var margin = doc.Variable(Margin);
            return new Size(size.Width - margin.Width, size.Height - margin.Height);
        });
        public static DocumentLocal<Size> Size { get; } = new DocumentLocal<Size>(new Size(595, 842));
        public static DocumentLocal<Thickness> Margin { get; } = new DocumentLocal<Thickness>(new Thickness(57));
    }
}
