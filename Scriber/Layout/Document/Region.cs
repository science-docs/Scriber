using System.Collections.Generic;
using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public class Region : DocumentElement
    {
        public ElementCollection<DocumentElement> Elements { get; }
        public bool Flexible { get; set; }
        public bool Glue { get; set; }

        private readonly Dictionary<Measurement, DocumentElement> elementMap = new Dictionary<Measurement, DocumentElement>();

        public Region()
        {
            Elements = new ElementCollection<DocumentElement>(this);
        }

        protected internal override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            elementMap[measurement].Render(drawingContext, measurement);
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

        protected override Measurement MeasureOverride(Size availableSize)
        {
            elementMap.Clear();
            var ms = new Measurement(this);

            foreach (var element in Elements)
            {
                ms.Subs.Add(element.Measure(availableSize));

                // TODO: this
                //foreach (var measurement in elementMs)
                //{
                //    var m = measurement.Copy(this);
                //    m.Flexible = Flexible;
                //    elementMap[m] = element;
                //    ms.Add(m);
                //}

                //if (elementMs.Count > 0 && Glue)
                //{
                //    elementMs[^1].PagebreakPenalty = double.PositiveInfinity;
                //}
            }

            //if (ms.Count > 0)
            //{
            //    var fm = ms[0].Margin;
            //    fm.Top += Margin.Top;
            //    var lm = ms[^1].Margin;
            //    lm.Bottom += Margin.Bottom;
            //    ms[0].Margin = fm;
            //    ms[^1].Margin = lm;

            //    ms[^1].PagebreakPenalty = 0;
            //}

            return ms;
        }
    }
}
