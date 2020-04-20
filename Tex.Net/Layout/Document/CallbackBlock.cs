using System;
using System.Collections.Generic;
using System.Text;
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

        public override void OnRender(IDrawingContext drawingContext)
        {
            throw new NotImplementedException();
        }

        protected override void ArrangeOverride(Rectangle finalRectangle)
        {
            throw new NotImplementedException();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            throw new NotImplementedException();
        }
    }
}
