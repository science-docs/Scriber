using System;
using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public delegate DocumentElement BlockCallback();

    public class CallbackBlock : DocumentElement
    {
        public BlockCallback Callback { get; }

        public CallbackBlock(BlockCallback callback)
        {
            Callback = callback;
        }

        protected override AbstractElement CloneInternal()
        {
            return new CallbackBlock(Callback);
        }

        protected override Measurements MeasureOverride(Size availableSize)
        {
            throw new NotImplementedException();
        }

        public override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            throw new NotImplementedException();
        }
    }
}
