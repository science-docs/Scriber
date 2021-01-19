using Scriber.Layout.Document;
using Scriber.Util;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class Style
    {
        public AbstractElement Element { get; }

        private readonly StyleContainer container = new StyleContainer(StyleOrigin.Author);
        private StyleContainer? computed;

        public Style(AbstractElement element)
        {
            Element = element;
        }

        [return: MaybeNull]
        public T Get<T>(ComputedStyleKey<T> key)
        {
            return key.Compute(this);
        }

        [return: MaybeNull]
        public T Get<T>(StyleKey<T> key)
        {
            return Get<T>((StyleKey)key);
        }

        [return: MaybeNull]
        public T Get<T>(StyleKey key)
        {
            var hasKey = TryGet<T>(key, out var result);
            if (!hasKey)
            {
                if (key.Default is T defaultValue)
                {
                    result = defaultValue;
                }
                else
                {
                    result = default;
                }
            }
            return result;
        }

        public bool TryGet<T>(StyleKey<T> key, [MaybeNull] out T result)
        {
            return TryGet((StyleKey)key, out result);
        }

        public bool TryGet<T>(StyleKey key, [MaybeNull] out T result)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (container.TryGet<T>(key, out var field))
            {
                result = field;
                return true;
            }

            if (computed == null)
            {
                computed = Element.Document!.Styles.Compute(Element);
            }

            if (computed.TryGet(key, out field))
            {
                result = field;
                return true;
            }
            else if (key.Inherited && Element.Parent != null)
            {
                result = Element.Parent.Style.Get<T>(key);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public void Set(StyleKey key, object field)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var fieldType = field.GetType();
            if (fieldType.IsAssignableFrom(key.Type))
            {
                throw new ArgumentException($"Cannot assign value of type {fieldType.FormattedName()} to property {key}.");
            }

            container.Set(key, field);
        }

        public void Set<T>(StyleKey<T> key, [DisallowNull] T field)
        {
            container.Set(key, field);
        }
    }
}
