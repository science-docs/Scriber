namespace Tex.Net.Text
{
    public class Hyphenator
    {
        //private iText.Layout.Hyphenation.Hyphenator hyphenator;

        public void Initialize(string langCode)
        {
            //hyphenator = new iText.Layout.Hyphenation.Hyphenator(langCode, null, 1, 1);
        }

        public int[] Hyphenate(string value)
        {
            return null;
            //return hyphenator.Hyphenate(value).GetHyphenationPoints();
        }
    }
}
