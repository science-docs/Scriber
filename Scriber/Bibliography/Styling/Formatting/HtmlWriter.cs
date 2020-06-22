using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Scriber.Bibliography.Styling.Formatting
{
    internal class HtmlWriter : IDisposable
    {
        // init
        private XmlWriter _Writer;
        private Stack<object> _Stack;

        public HtmlWriter(XmlWriter writer)
        {
            // init
            this._Writer = writer;
            this._Stack = new Stack<object>();
        }
        public void Dispose()
        {
            // close stack
            while (this._Stack.Count > 0)
            {
                // pop
                this._Stack.Pop();

                // write end tag
                this._Writer.WriteEndElement();
            }

            // done
            this._Writer = null;
            this._Stack = null;
        }

        public void Add(TextRun run)
        {
            // font style
            this.SetStyle(run.FontStyle, FontStyle.Normal, r =>
            {
                switch (r)
                {
                    case FontStyle.Italic:
                        this._Writer.WriteStartElement("i");
                        break;
                    case FontStyle.Oblique:
                        this._Writer.WriteStartElement("span");
                        this._Writer.WriteAttributeString("style", "font-style: oblique;");
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            // font variant
            this.SetStyle(run.FontVariant, FontVariant.Normal, r =>
            {
                switch (r)
                {
                    case FontVariant.SmallCaps:
                        this._Writer.WriteStartElement("span");
                        this._Writer.WriteAttributeString("style", "font-variant:small-caps;");
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            // font weight
            this.SetStyle(run.FontWeight, FontWeight.Normal, r =>
            {
                switch (r)
                {
                    case FontWeight.Bold:
                        this._Writer.WriteStartElement("b");
                        break;
                    case FontWeight.Light:
                        this._Writer.WriteStartElement("span");
                        this._Writer.WriteAttributeString("style", "font-weight: lighter;");
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            // text decoration
            this.SetStyle(run.TextDecoration, TextDecoration.None, r =>
            {
                switch (r)
                {
                    case TextDecoration.Underline:
                        this._Writer.WriteStartElement("u");
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            // vertical align
            this.SetStyle(run.VerticalAlign, VerticalAlign.Baseline, r =>
            {
                switch (r)
                {
                    case VerticalAlign.Subscript:
                        this._Writer.WriteStartElement("sub");
                        break;
                    case VerticalAlign.Superscript:
                        this._Writer.WriteStartElement("sup");
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            // add
            this._Writer.WriteValue(run.Text);
        }
        private void SetStyle<T>(T requested, T defaultValue, Action<T> openTag)
        {
            // init
            var current = (this._Stack.Any(x => x is T) ? this._Stack.Single(x => x is T) : defaultValue)!;

            // close tag?
            if (!current.Equals(requested))
            {
                // pop stack
                while (this._Stack.Any(x => x is T))
                {
                    // pop
                    var tag = this._Stack.Pop();

                    // close
                    this._Writer.WriteEndElement();
                }

                // open tag?
                if (!requested.Equals(defaultValue))
                {
                    // open tag
                    openTag(requested);

                    // push stack
                    this._Stack.Push(requested);
                }
            }
        }
    }
}
