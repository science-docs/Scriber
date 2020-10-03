using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Scriber
{
    public static class DocumentLocal
    {
        public static int Increment(this DocumentLocal<int> local, Document document)
        {
            var value = local[document];
            local[document] = ++value;
            return value;
        }

        public static DocumentLocal<T> Create<T>() where T : new()
        {
            return new DocumentLocal<T>(() => new T());
        }
    }

    public class DocumentLocal<T>
    {
        private readonly ConditionalWeakTable<Document, ObjectBox<T>> table = new ConditionalWeakTable<Document, ObjectBox<T>>();
        private readonly Func<T> consumer;

        public DocumentLocal([AllowNull] T defaultValue)
        {
            // As Strings cannot be manipulated, they are valid types for a document local value
            if (defaultValue != null && !defaultValue.GetType().IsValueType && defaultValue.GetType() != typeof(string))
            {
                throw new ArgumentException("Default value of a document local has to be a value type", nameof(defaultValue));
            }

            if (defaultValue == null)
            {
                consumer = CreateDefault;
            }
            else
            {
                consumer = () => defaultValue;
            }
        }

        public DocumentLocal(Func<T> consumer)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException(nameof(consumer));
            }

            if (consumer() == null)
            {
                throw new ArgumentException("Created object from consumer cannot be null.", nameof(consumer));
            }

            this.consumer = consumer;
        }

        protected DocumentLocal()
        {
            consumer = CreateDefault;
        }

        public T this[Document document]
        {
            get => Get(document);
            set => Set(document, value);
        }

        public virtual T Get(Document document)
        {
            if (table.TryGetValue(document, out var value))
            {
                return value.Element;
            }
            else
            {
                var consumed = consumer();
                Set(document, consumed);
                return consumed;
            }
        }

        public virtual void Set(Document document, [AllowNull] T value)
        {
            table.AddOrUpdate(document, new ObjectBox<T>(value));
        }

        [return: MaybeNull]
        private static T CreateDefault()
        {
            return default;
        }

        private class ObjectBox<TElement>
        {
            [AllowNull]
            public TElement Element { get; set; }

            public ObjectBox([AllowNull] TElement element)
            {
                Element = element;
            }
        }
    }
}
