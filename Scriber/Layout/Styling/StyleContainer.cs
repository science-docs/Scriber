﻿using System.Collections;
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

    public class StyleContainer : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

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
