using System;
using System.Diagnostics.CodeAnalysis;

namespace Scriber
{
    public class ComputedDocumentLocal<T> : DocumentLocal<T>
    {
        private readonly Func<Document, T> computer;

        public ComputedDocumentLocal(Func<Document, T> computer) : base()
        {
            this.computer = computer;
        }

        public override T Get(Document document)
        {
            return computer(document);
        }

        public override void Set(Document document, [AllowNull] T value)
        {
            throw new NotSupportedException();
        }
    }
}
