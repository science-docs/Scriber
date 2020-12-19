﻿using Scriber.Text;
using System;
using System.Collections.Generic;

namespace Scriber.Layout.Document
{
    public class FootnoteLeaf : Leaf, ITextLeaf
    {
        public string Content { get; }
        public Paragraph Element { get; }

        public override IEnumerable<Symbol> Symbols => Element.Symbols;

        public FootnoteLeaf(string content, Paragraph value) : this(content, value, true)
        {
        }

        private FootnoteLeaf(string content, Paragraph value, bool addPrefix)
        {
            FontStyle = FontStyle.Superscript;
            Content = content;
            Element = value ?? throw new ArgumentNullException(nameof(value));
            value.VerticalAlignment = VerticalAlignment.Bottom;

            if (addPrefix)
            {
                value.Leaves.Insert(0, new TextLeaf(content) { FontStyle = FontStyle.Superscript });
            }
        }

        public override LineNode[] GetNodes()
        {
            Element.Parent = Parent;
            return LineNodeTransformer.Create(this).ToArray();
        }

        protected override AbstractElement CloneInternal()
        {
            return new FootnoteLeaf(Content, Element.Clone(), false);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Font == null)
            {
                throw new LayoutException("Font property is null");
            }

            var height = FontSize;
            var width = Font.GetWidth(Content, FontSize, FontWeight);
            return new Size(width, height);
        }
    }
}
