using Scriber.Layout.Styling;
using Scriber.Text;
using System;
using System.Collections.Generic;

namespace Scriber.Layout.Document
{
    public abstract class AbstractElement
    {
        public AbstractElement? Parent { get; set; }

        public Style Style
        {
            get
            {
                style ??= new Style(this);
                return style;
            }
        }

        public FontWeight FontWeight
        {
            get => fontWeight ?? Parent?.FontWeight ?? FontWeight.Normal;
            set => fontWeight = value;
        }

        public FontStyle FontStyle
        {
            get => fontStyle ?? Parent?.FontStyle ?? FontStyle.Normal;
            set => fontStyle = value;
        }

        public Color Foreground
        {
            get => foreground ?? Parent?.Foreground ?? Colors.Black;
            set => foreground = value;
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment ?? Parent?.HorizontalAlignment ?? 0;
            set => horizontalAlignment = value;
        }

        public VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment ?? Parent?.VerticalAlignment ?? 0;
            set => verticalAlignment = value;
        }

        public DocumentPage? Page
        {
            get => page ?? Parent?.Page;
            set => page = value;
        }

        public Scriber.Document? Document
        {
            get => document ?? Parent?.Document;
            set => document = value;
        }

        public virtual IEnumerable<Symbol> Symbols => Array.Empty<Symbol>();

        public string Tag { get; set; } = string.Empty;
        public List<string> Classes { get; } = new List<string>();
        
        //public Thickness Margin { get; set; }

        private FontStyle? fontStyle;
        private Color? foreground;
        private FontWeight? fontWeight;
        private HorizontalAlignment? horizontalAlignment;
        private VerticalAlignment? verticalAlignment;
        private DocumentPage? page;
        private Scriber.Document? document;
        private Style? style;

        public AbstractElement Clone()
        {
            var clone = CloneInternal();
            clone.Tag = Tag;
            clone.Classes.AddRange(Classes);
            return clone;
        }

        protected abstract AbstractElement CloneInternal();

        public virtual void Interlude()
        {

        }
    }
}
