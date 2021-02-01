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
                    selectorCount = styles.Count();
                }

                array = new BitArray(selectorCount);
            }
            
            int index = 0;
            foreach (var container in styles)
            {
                array.Set(index++, container.Selector.Matches(element));
            }

            if (computedStyles.TryGetValue(array, out var computedContainer))
            {
                return computedContainer;
            }
            else
            {
                var container = new StyleContainer(AllStyleSelector.Singleton);

                foreach (var styleContainer in styles)
                {
                    if (styleContainer.Selector.Matches(element))
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
