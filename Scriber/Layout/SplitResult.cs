namespace Scriber.Layout
{
    public class SplitResult
    {
        public Measurement Measurement { get; }
        public Measurement Source { get; }
        public Measurement? Next { get; }
        public bool HasNext => Next != null;
        public bool IsSplit => Measurement != Source;

        public SplitResult(Measurement source, Measurement measurement, Measurement? next)
        {
            Measurement = measurement;
            Source = source;
            Next = next;
        }

        public static SplitResult Fail(Measurement source)
        {
            return new SplitResult(source, source, null);
        }
    }
}
