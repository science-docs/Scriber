using Scriber.Layout.Document;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Scriber.Layout.Styling
{
    public class StyleCollection : IEnumerable<StyleContainer>
    {
        private readonly Dictionary<StyleSelector, StyleContainer> styles = new Dictionary<StyleSelector, StyleContainer>();
        private readonly Dictionary<int, List<StyleSelector>> selectorsBuckets = new Dictionary<int, List<StyleSelector>>();

        public StyleCollection()
        {
            for (int i = 0; i <= StyleSelector.MaxSpecificity; i++)
            {
                selectorsBuckets[i] = new List<StyleSelector>();
            }
        }

        public void Add(StyleContainer styleContainer)
        {
            foreach (var selector in styleContainer.Selectors)
            {
                styles[selector] = styleContainer;
                selectorsBuckets[selector.Specificity].Add(selector);
            }
        }

        public IEnumerator<StyleContainer> GetEnumerator()
        {
            return styles.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool TryGetValue<T>(AbstractElement element, StyleKey key, [MaybeNullWhen(false)] out T value)
        {
            StyleContainer? style = null;
            for (int i = StyleSelector.MaxSpecificity; i > -1; i--)
            {
                var bucket = selectorsBuckets[i];
                var firstSelector = bucket.FirstOrDefault(e => e.Matches(element) && styles[e].ContainsKey(key));
                if (firstSelector != null)
                {
                    style = styles[firstSelector];
                    break;
                }
            }

            if (style != null && style.Get<T>(key) is T result)
            {
                value = result;
                return true;
            }

            value = default;
            return false;
        }

        
    }
}
