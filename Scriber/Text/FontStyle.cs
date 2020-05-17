namespace Scriber.Text
{
    public enum FontStyle
    {
        Normal,
        Superscript,
        Subscript
    }

    public class FontStyler
    {
        public static double ScaleSize(FontStyle style, double size)
        {
            if (style != FontStyle.Normal)
            {
                return size * 0.58;
            }
            else
            {
                return size;
            }
        }

        public static double ScaleOffset(FontStyle style, double offset)
        {
            if (style == FontStyle.Superscript)
            {
                return offset * 0.66;
            }
            else if (style == FontStyle.Subscript)
            {
                return offset * 1.33;
            }
            else
            {
                return offset;
            }
        }
    }
}
