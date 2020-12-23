using Scriber.Layout.Document;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class Style
    {
        public AbstractElement Element { get; }

        private readonly Dictionary<StyleKey, object> fields = new Dictionary<StyleKey, object>();

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
            if (fields.TryGetValue(key, out var field) && field is T result)
            {
                return result;
            }
            else if (Element.Document!.Styles.TryGetValue(Element, key, out T computedResult))
            {
                return computedResult;
            }
            else if (Element.Parent != null)
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
            fields[key] = field;
        }
    }
}
