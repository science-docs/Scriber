using Scriber.Layout.Document;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Layout.Styling
{
    public class StyleCollection : IEnumerable<StyleContainer>
    {
        private readonly List<StyleContainer> styles = new List<StyleContainer>();
        private readonly StyleCache cache;

        public StyleCollection()
        {
            cache = new StyleCache(this);
        }

        public void Add(StyleContainer styleContainer)
        {
            styles.Add(styleContainer);
        }

        public IEnumerator<StyleContainer> GetEnumerator()
        {
            return styles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public StyleContainer Compute(AbstractElement element)
        {
            return cache.Compute(element);
        }

        //public StyleContainer Merge(AbstractElement element)
        //{
        //    var container = new StyleContainer(StyleOrigin.Engine);

        //    foreach (var styleContainer in styles)
        //    {
        //        if (styleContainer.Selectors.Any(e => e.Matches(element)))
        //        {
        //            foreach (var (key, value) in styleContainer)
        //            {
        //                container.Set(key, value);
        //            }
        //        }
        //    }

        //    var parent = element.Parent;
        //    if (parent != null)
        //    {
        //        var parentContainer = Compute(parent);

        //        foreach (var (key, value) in parentContainer)
        //        {
        //            if (key.Inherited && !container.ContainsKey(key))
        //            {
        //                container.Set(key, value);
        //            }
        //        }
        //    }

        //    return container;
        //}
    }
}
