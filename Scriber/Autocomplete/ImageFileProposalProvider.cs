namespace Scriber.Autocomplete
{
    public class ImageFileProposalProvider : FileProposalProvider
    {
        public ImageFileProposalProvider() : base(BuildFilter("png", "jpeg", "jpg", "bmp", "gif"))
        {
        }
    }
}
