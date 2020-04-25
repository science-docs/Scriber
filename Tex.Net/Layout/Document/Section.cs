using System.Collections.Generic;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public class Section : DocumentElement
    {
        public ElementCollection<DocumentElement> Elements { get; }

        private readonly Dictionary<Measurement, DocumentElement> elementMap = new Dictionary<Measurement, DocumentElement>();

        public Section()
        {
            Elements = new ElementCollection<DocumentElement>(this);
        }

        public override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            elementMap[measurement].OnRender(drawingContext, measurement);
        }

        protected override AbstractElement CloneInternal()
        {
            var section = new Section();
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
                var mss = element.Measure(availableSize);

                foreach (var m in mss)
                {
                    elementMap[m] = element;
                    ms.Add(m);
                }
            }

            return ms;
        }
    }
}
