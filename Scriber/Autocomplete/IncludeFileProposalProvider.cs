namespace Scriber.Autocomplete
{
    public class IncludeFileProposalProvider : FileProposalProvider
    {
        public IncludeFileProposalProvider() : base(BuildFilter("sc"))
        {

        }
    }
}
