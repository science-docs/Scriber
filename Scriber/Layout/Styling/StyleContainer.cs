using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{

    public class StyleContainer : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        public StyleSelector Selector { get; }

        public StyleContainer(string selectorText)
        {
            Selector = StyleSelector.FromString(selectorText);
        }

        public StyleContainer(StyleSelector selectors)
        {
            Selector = selectors;
        }

        public bool ContainsKey(string key)
        {
            return fields.ContainsKey(key);
        }

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T t)
        {
            if (fields.TryGetValue(key, out var result) && result is T generic)
            {
                t = generic;
                return true;
            }
            else
            {
                t = default;
                return false;
            }
        }

        [return: MaybeNull]
        public T Get<T>(string key)
        {
            if (fields.TryGetValue(key, out var result) && result is T t)
            {
                return t;
            }
            else
            {
                return default;
            }
        }

        public void Set(IStyleKey key, object value)
        {
            Set(key.Name, value);
        }

        public void Set(string key, object value)
        {
            fields[key] = value;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
