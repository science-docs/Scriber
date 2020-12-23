using Scriber.Layout.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class StyleCollection
    {
        private readonly List<StyleContainer> styles = new List<StyleContainer>();

        public void Add(StyleContainer styleContainer)
        {
            styles.Add(styleContainer);
        }

        public bool TryGetValue<T>(AbstractElement element, StyleKey key, [MaybeNull] out T value)
        {
            foreach (var container in styles)
            {
                if (container.ContainsKey(key) && container.Matches(element))
                {
                    value = container.Get<T>(key);
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
