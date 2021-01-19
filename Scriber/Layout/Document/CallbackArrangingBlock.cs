using System;

namespace Scriber.Layout.Document
{
    public class CallbackArrangingBlock : ArrangingBlock
    {
        public Action<DocumentElement> Callback { get; }

        public CallbackArrangingBlock(Action<DocumentElement> callback)
        {
            Callback = callback;
        }

        public override void Manipulate(Scriber.Document document)
        {
            Callback(this);
        }

        protected override AbstractElement CloneInternal()
        {
            return new CallbackArrangingBlock(Callback);
        }
    }
}
