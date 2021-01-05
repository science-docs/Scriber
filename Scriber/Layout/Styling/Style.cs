using Scriber.Layout.Document;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class Style
    {
        public AbstractElement Element { get; }

        private readonly StyleContainer container = new StyleContainer(StyleOrigin.Author);

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
            if (container.TryGet<T>(key, out var field))
            {
                return field;
            }
            else if (Element.Document!.Styles.TryGetValue(Element, key, out T computedResult))
            {
                return computedResult;
            }
            else if (key.Inherited && Element.Parent != null)
            {
                return Element.Parent.Style.Get<T>(key);
            }
            else if (key.Default is T defaultValue)
            {
                return defaultValue;
            }
            else
            {
                return default;
            }
        }

        public void Set(StyleKey key, object field)
        {
            container.Set(key, field);
        }

        public void Set<T>(StyleKey<T> key, [DisallowNull] T field)
        {
            container.Set(key, field);
        }
    }
}
