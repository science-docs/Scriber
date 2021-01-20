using Scriber.Layout.Document;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Layout
{
    public class Measurement
    {
        public DocumentElement Element { get; }
        public bool Flexible { get; set; }
        public Size Size
        {
            get => size ?? GetSubSize();
            set => size = value;
        }
        public Thickness Margin { get; set; }
        public object? Tag { get; set; }
        public Position Position { get; internal set; }

        public List<Measurement> Subs { get; }
        /// <summary>
        /// Extra measurements are used for elements outside the actual element. These are e.g. footnotes.
        /// </summary>
        public Measurement Extra => extra.Value;
        public Measurement AccumulatedExtra
        {
            get
            {
                var ms = new Measurement(Element);
                ms.Subs.AddRange(AccumulateExtras());
                return ms;
            }
        }

        public Size TotalSize => new Size(Size.Width + Margin.Width, Size.Height + Margin.Height);

        private Size? size;
        private readonly Lazy<Measurement> extra;

        public Measurement(DocumentElement element) : this(element, null, Thickness.Zero)
        {
        }

        public Measurement(DocumentElement element, Size? size, Thickness margin)
        {
            Subs = new List<Measurement>();
            Element = element;
            this.size = size;
            Margin = margin;
            extra = new Lazy<Measurement>(() => new Measurement(Element));
        }

        public Measurement Copy(DocumentElement element)
        {
            return new Measurement(element, Size, Margin)
            {
                Flexible = Flexible,
                Position = Position
            };
        }

        private Size GetSubSize()
        {
            double width = 0;
            double height = 0;
            foreach (var sub in Subs)
            {
                width = Math.Max(width, sub.TotalSize.Width);
                height += sub.TotalSize.Height;
            }

            return new Size(width, height);
        }

        private IEnumerable<Measurement> AccumulateExtras()
        {
            var subExtras = Subs.SelectMany(e => e.AccumulateExtras());
            if (extra.IsValueCreated)
            {
                return Extra.Subs.Concat(subExtras);
            }
            else
            {
                return subExtras;
            }
        }
    }
}
