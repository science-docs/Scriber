using Tex.Net.Layout;

namespace Tex.Net
{
    public static class DocumentVariables
    {
        public static string Page = "page";
        public static string Size = "size";
        public static string Margin = "margin";
        public static string Length = "length";
        public static string TableOfContent = "toc";

        public static string BaselineStretch = "baselinestretch";

        public static void Setup(DocumentVariable vars)
        {
            vars[Page][Size].SetValue(new Size(595, 842));
            vars[Page][Margin].SetValue(new Thickness(57));

            vars[Length]["parindent"].SetValue(0.0);
            vars[Length]["parskip"].SetValue(0.0);
            vars[Length][BaselineStretch].SetValue(1.2);
        }
    }
}
