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

        public Color Foreground
        {
            get => foreground ?? Parent?.Foreground ?? Colors.Black;
            set => foreground = value;
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

        public bool IsValid { get; protected set; } = false;

        public virtual IEnumerable<Symbol> Symbols => Array.Empty<Symbol>();

        public string Tag { get; set; } = string.Empty;
        public List<string> Classes { get; } = new List<string>();

        private Color? foreground;
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

        public virtual IEnumerable<AbstractElement> ChildElements()
        {
            yield break;
        }

        protected abstract AbstractElement CloneInternal();

        public virtual void Interlude()
        {

        }

        public void Invalidate()
        {
            IsValid = false;
            Parent?.Invalidate();
        }
    }
}
