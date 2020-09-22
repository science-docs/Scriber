namespace Scriber.Autocomplete
{
    public class IncludePdfFileProposalProvider : FileProposalProvider
    {
        public IncludePdfFileProposalProvider() : base(BuildFilter("pdf"))
        {

        }
    }
}
