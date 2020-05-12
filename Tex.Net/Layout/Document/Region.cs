using System.Collections.Generic;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public class Region : DocumentElement
    {
        public ElementCollection<DocumentElement> Elements { get; }
        public bool Flexible { get; set; }

        private readonly Dictionary<Measurement, DocumentElement> elementMap = new Dictionary<Measurement, DocumentElement>();

        public Region()
        {
            Elements = new ElementCollection<DocumentElement>(this);
        }

        public override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            elementMap[measurement].OnRender(drawingContext, measurement);
        }

        protected override AbstractElement CloneInternal()
        {
            var section = new Region();
            foreach (var element in Elements)
            {
                section.Elements.Add(element.Clone());
            }
            return section;
        }

        protected override Measurements MeasureOverride(Size availableSize)
        {
            elementMap.Clear();
            var ms = new Measurements();

            foreach (var element in Elements)
            {
                var elementMs = element.Measure(availableSize);

                foreach (var measurement in elementMs)
                {
                    measurement.Flexible = Flexible;
                    elementMap[measurement] = element;
                    ms.Add(measurement);
                }

                if (elementMs.Count > 0)
                {
                    elementMs[^1].PagebreakPenalty = double.PositiveInfinity;
                }
            }

            if (ms.Count > 0)
            {
                var fm = ms[0].Margin;
                fm.Top += Margin.Top;
                var lm = ms[^1].Margin;
                lm.Bottom += Margin.Bottom;
                ms[0].Margin = fm;
                ms[^1].Margin = lm;

                ms[^1].PagebreakPenalty = 0;
            }

            return ms;
        }
    }
}
