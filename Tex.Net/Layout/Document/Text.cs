using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Drawing;
using Tex.Net.Text;

namespace Tex.Net.Layout.Document
{
    public class Text : Leaf
    {
        public string Content { get; set; }

        public override LineNode[] GetNodes()
        {
            return LineNodeTransformer.Create(this, Content).ToArray();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var height = FontSize;
            var width = Font.GetWidth(Content, FontSize);
            return new Size(width, height);
        }
    }
}
