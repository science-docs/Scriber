using Scriber.Layout.Document;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
            var container = styles.AsParallel().FirstOrDefault(e => e.ContainsKey(key) && e.Matches(element));

            if (container != null)
            {
                value = container.Get<T>(key);
                return true;
            }

            value = default;
            return false;
        }
    }
}
