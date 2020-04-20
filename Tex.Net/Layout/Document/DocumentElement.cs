using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tex.Net.Drawing;
using Tex.Net.Text;

namespace Tex.Net.Layout.Document
{
    public abstract class DocumentElement
    {
        public DocumentElement Parent { get; set; }

        public Font Font
        {
            get => font ?? Parent?.Font;
            set => font = value;
        }
        public double FontSize
        {
            get => fontSize ?? Parent?.FontSize ?? 0;
            set => fontSize = value;
        }
        public FontWeight FontWeight
        {
            get => fontWeight ?? Parent?.FontWeight ?? FontWeight.Normal;
            set => fontWeight = value;
        }
        public Color Foreground
        {
            get => foreground ?? Parent?.Foreground ?? Color.Black;
            set => foreground = value;
        }

        public Thickness Margin
        {
            get => margin ?? Parent?.Margin ?? Thickness.Zero;
            set => margin = value;
        }
        
        private Font font;
        private double? fontSize;
        private Color? foreground;
        private FontWeight? fontWeight;
        private Thickness? margin;
    }
}
