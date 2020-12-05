namespace Scriber.Variables
{
    public static class ParagraphVariables
    {
        public static DocumentLocal<double> BaselineStretch { get; } = new DocumentLocal<double>(1.25);
        public static DocumentLocal<double> Skip { get; } = new DocumentLocal<double>(8);
        public static DocumentLocal<double> Indent { get; } = new DocumentLocal<double>(0);
    }
}
