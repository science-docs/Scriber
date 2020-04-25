using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Layout.Document
{
    public class CallbackArrangingBlock : ArrangingBlock
    {
        public Action<Block> Callback { get; }

        public CallbackArrangingBlock(Action<Block> callback)
        {
            Callback = callback;
        }

        public override void Manipulate(Net.Document document)
        {
            Callback(this);
        }

        protected override DocumentElement CloneInternal()
        {
            return new CallbackArrangingBlock(Callback);
        }
    }
}
