using System;

namespace Scriber.Layout.Document
{
    public class CallbackMeasuringBlock : MeasuringBlock
    {
        public Action<DocumentElement> Callback { get; }

        public CallbackMeasuringBlock(Action<DocumentElement> callback)
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
