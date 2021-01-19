namespace Scriber.Autocomplete
{
    public class BibliographyFileProposalProvider : FileProposalProvider
    {
        public BibliographyFileProposalProvider() : base(BuildFilter("bib", "sc"))
        {
        }
    }
}
