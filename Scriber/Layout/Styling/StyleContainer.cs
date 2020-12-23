using Scriber.Layout.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        }

        public StyleContainer(StyleOrigin origin, params StyleSelector[] selectors)
        {
            Origin = origin;
            Selectors = selectors;
        }

        public bool Matches(AbstractElement element)
        {
            for (int i = 0; i < Selectors.Count; i++)
            {
                if (Selectors[i].Matches(element))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(StyleKey key)
        {
            return fields.ContainsKey(key);
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
