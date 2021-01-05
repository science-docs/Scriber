using Scriber.Layout.Document;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Scriber.Layout.Styling
{
    public enum StyleOrigin
    {
        Engine,
        Template,
        Author
    }

    public class StyleContainer
    {
        private readonly Dictionary<StyleKey, object> fields = new Dictionary<StyleKey, object>();

        public StyleOrigin Origin { get; }
        public IReadOnlyList<StyleSelector> Selectors { get; }

        public StyleContainer(StyleOrigin origin, string selectorText)
        {
            Origin = origin;
            Selectors = StyleSelector.FromString(selectorText).ToArray();
        }

        public StyleContainer(StyleOrigin origin, params StyleSelector[] selectors)
        {
            Origin = origin;
            Selectors = selectors;
        }

        public bool ContainsKey(StyleKey key)
        {
            return fields.ContainsKey(key);
        }

        public bool TryGet<T>(StyleKey key, [MaybeNullWhen(false)] out T t)
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
        public T Get<T>(StyleKey key)
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

        public void Set<T>(StyleKey<T> key, [DisallowNull] T value)
        {
            fields[key] = value;
        }

        public void Set(StyleKey key, object value)
        {
            fields[key] = value;
        }
    }
}
