using Scriber.Layout.Document;
using System.Collections;
using System.Collections.Generic;

namespace Scriber.Layout.Styling
{
    public class StyleCollection : IEnumerable<StyleContainer>
    {
        private readonly SortedList<Priority, StyleContainer> styles = new SortedList<Priority, StyleContainer>(new DuplicatePriorityComparer());
        private readonly StyleCache cache;

        public StyleCollection()
        {
            cache = new StyleCache(this);
        }

        public void Add(StyleContainer styleContainer)
        {
            styles.Add(styleContainer.Selector.Specificity, styleContainer);
        }

        public IEnumerator<StyleContainer> GetEnumerator()
        {
            return styles.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public StyleContainer Compute(AbstractElement element)
        {
            return cache.Compute(element);
        }

        private class DuplicatePriorityComparer : IComparer<Priority>
        {
            public int Compare(Priority x, Priority y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;
                else
                    return result;
            }
        }
    }
}
