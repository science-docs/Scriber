using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tex.Net.Text;

namespace Tex.Net.Layout.Document
{
    public abstract class DocumentElement
    {
        public DocumentElement Parent { get; set; }

        public Font Font { get; set; }
        public double FontSize { get; set; }
        public Color Foreground { get; set; }
    }
}
