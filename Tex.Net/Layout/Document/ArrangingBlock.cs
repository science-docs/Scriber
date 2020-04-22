using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    /// <summary>
    /// Base class for block element that manipulate the document during the arrange phase.
    /// </summary>
    public abstract class ArrangingBlock : Block
    {
        public override bool IsVisible => false;

        public abstract void Manipulate(Net.Document document);

        public override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {

        }

        protected override Measurements MeasureOverride(Size availableSize)
        {
            Manipulate(Page.Document);
            return new Measurements();
        }
    }
}
