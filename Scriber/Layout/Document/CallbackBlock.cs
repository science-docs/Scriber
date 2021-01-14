using System;
using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public class CallbackBlock : DocumentElement
    {
        public Func<DocumentElement> Callback { get; }

        public CallbackBlock(Func<DocumentElement> callback)
        {
            Callback = callback;
        }

        protected override AbstractElement CloneInternal()
        {
            return new CallbackBlock(Callback);
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            throw new NotSupportedException();
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            throw new NotSupportedException();
        }
    }
}
