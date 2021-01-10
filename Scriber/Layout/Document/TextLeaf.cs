using Scriber.Layout.Styling;
using Scriber.Text;
using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Layout.Document
{
    public class TextLeaf : Leaf, ITextLeaf
    {
        public string? Content { get; set; }

        public override IEnumerable<Symbol> Symbols
        {
            get
            {
                if (Content is null)
                    return Array.Empty<Symbol>();

                var symbols = Content.Split(e => !char.IsLetterOrDigit(e), StringSplitOptions.RemoveEmptyEntries);
                return symbols.Where(e => e.IsWord()).Select(e => new Symbol(SymbolType.Word, e));
            }
        }

        public TextLeaf()
        {

        }

        public TextLeaf(string? content)
        {
            Content = content;
        }

        protected override AbstractElement CloneInternal()
        {
            return new TextLeaf(Content);
        }

        public override LineNode[] GetNodes()
        {
            if (Content == null)
            {
                throw new LayoutException("Content property is null");
            }

            return LineNodeTransformer.Create(this).ToArray();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Content == null)
            {
                throw new LayoutException("Content property is null");
            }
            var font = Style.Get(StyleKeys.Font);
            if (font == null)
            {
                throw new LayoutException("Font property is null");
            }

            var height = Style.Get(StyleKeys.FontSize).Point;
            var width = font.GetWidth(Content, height, Style.Get(StyleKeys.FontWeight));
            return new Size(width, height);
        }
    }
}
