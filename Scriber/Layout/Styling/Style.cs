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

        public T Get<T>(AbstractStyleKey<T> key)
        {
            return key.Get(this);
        }

        public T Get<T>(string key, bool inherited, T defaultValue)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (container.TryGet<T>(key, out var containerField))
            {
                return containerField;
            }

            if (computed == null)
            {
                computed = Element.Document!.Styles.Compute(Element);
            }

            if (computed.TryGet<T>(key, out var computedField))
            {
                return computedField;
            }
            else if (inherited && Element.Parent != null)
            {
                return Element.Parent.Style.Get(key, inherited, defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        public void Set<T>(StyleKey<T> key, [DisallowNull] T field)
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
            if (!fieldType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException($"Cannot assign value of type {fieldType.FormattedName()} to property {key.Name}.");
            }

            container.Set(key.Name, field);
        }
    }
}
