using System;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public delegate Block BlockCallback();

    public class CallbackBlock : Block
    {
        public BlockCallback Callback { get; }

        public CallbackBlock(BlockCallback callback)
        {
            Callback = callback;
        }

        protected override DocumentElement CloneInternal()
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
