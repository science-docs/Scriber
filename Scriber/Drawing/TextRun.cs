using Scriber.Text;

namespace Scriber.Drawing
{
    public class TextRun
    {
        public string Text { get; set; }
        public Typeface Typeface { get; set; }

        public TextRun(string text, Typeface typeface)
        {
            Text = text;
            Typeface = typeface;
        }
    }
}
