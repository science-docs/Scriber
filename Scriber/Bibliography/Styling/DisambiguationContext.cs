namespace Scriber.Bibliography.Styling
{
    public class DisambiguationContext
    {
        public int MinAddNames
        {
            get;
            set;
        }
        public DisambiguateAddGivenNameLevel AddGivenNameLevel
        {
            get;
            set;
        }
    }

    public enum DisambiguateAddGivenNameLevel
    {
        None,
        Long,
        LongAndUninitialized
    }
}
