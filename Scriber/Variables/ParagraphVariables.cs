using Scriber.Engine;

namespace Scriber.Variables
{
    [Package]
    public static class ParagraphVariables
    {
        [Variable("Paragraph.BaselineStretch")]
        public static DocumentLocal<double> BaselineStretch { get; } = new DocumentLocal<double>(1.25);
        [Variable("Paragraph.Skip")]
        public static DocumentLocal<double> Skip { get; } = new DocumentLocal<double>(8);
        [Variable("Paragraph.Indent")]
        public static DocumentLocal<double> Indent { get; } = new DocumentLocal<double>(0);
    }
}
