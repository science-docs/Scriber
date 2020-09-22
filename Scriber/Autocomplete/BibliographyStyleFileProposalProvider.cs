namespace Scriber.Autocomplete
{
    public class BibliographyStyleFileProposalProvider : FileProposalProvider
    {
        public BibliographyStyleFileProposalProvider() : base(BuildFilter("csl"))
        {

        }
    }
}
