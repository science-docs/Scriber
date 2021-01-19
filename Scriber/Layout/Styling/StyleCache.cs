using Scriber.Layout.Document;
using Scriber.Util;
using System.Collections.Concurrent;
using System.Linq;

namespace Scriber.Layout.Styling
{
    public class StyleCache
    {
        private readonly ConcurrentDictionary<BitArray, StyleContainer> computedStyles = new ConcurrentDictionary<BitArray, StyleContainer>();
        private readonly StyleCollection styles;
        private int selectorCount = -1;

        public StyleCache(StyleCollection styles)
        {
            this.styles = styles;
        }

        public StyleContainer Compute(AbstractElement element)
        {
            BitArray array;
            lock (computedStyles)
            {
                if (selectorCount < 0)
                {
                    lock (computedStyles)
                    {
                        selectorCount = 0;
                        foreach (var container in styles)
                        {
                            selectorCount += container.Selectors.Count;
                        }
                    }
                }

                array = new BitArray(selectorCount);
            }
            
            int index = 0;
            foreach (var container in styles)
            {
                foreach (var selector in container.Selectors)
                {
                    array.Set(index++, selector.Matches(element));
                }
            }

            if (computedStyles.TryGetValue(array, out var computedContainer))
            {
                return computedContainer;
            }
            else
            {
                var container = new StyleContainer(StyleOrigin.Engine);

                foreach (var styleContainer in styles)
                {
                    if (styleContainer.Selectors.Any(e => e.Matches(element)))
                    {
                        foreach (var (key, value) in styleContainer)
                        {
                            container.Set(key, value);
                        }
                    }
                }

                computedStyles[array] = container;
                return container;
            }
        }
    }
}
