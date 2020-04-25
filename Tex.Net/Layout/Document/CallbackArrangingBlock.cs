using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Layout.Document
{
    public class CallbackArrangingBlock : ArrangingBlock
    {
        public Action<DocumentElement> Callback { get; }

        public CallbackArrangingBlock(Action<DocumentElement> callback)
        {
            Callback = callback;
        }

        public override void Manipulate(Net.Document document)
        {
            Callback(this);
        }

        protected override AbstractElement CloneInternal()
        {
            return new CallbackArrangingBlock(Callback);
        }
    }
}
