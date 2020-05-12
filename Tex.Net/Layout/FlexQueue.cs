using System;
using System.Collections.Generic;

namespace Tex.Net.Layout
{
    public class FlexQueue<T>
    {
        private readonly LinkedList<LinkedList<T>> list = new LinkedList<LinkedList<T>>();
        private readonly LinkedList<bool> flex = new LinkedList<bool>();

        public int Count => list.Count;
        public bool CanDequeue => Count > 0;

        public bool IsFlex => flex.First?.Value ?? false;

        public void Enqueue(bool flex)
        {
            if (flex)
            {
                this.flex.AddLast(flex);
                list.AddLast(new LinkedList<T>());
            }
            else
            {
                this.flex.AddFirst(flex);
                list.AddFirst(new LinkedList<T>());
            }
        }

        public void Enqueue(T item, bool flex)
        {
            if (flex)
            {
                list.Last?.Value.AddLast(item);
            }
            else
            {
                list.First?.Value.AddLast(item);
            }
        }

        public void Enqueue(IEnumerable<T> items, bool flex)
        {
            var l = new LinkedList<T>(items);
            if (flex)
            {
                this.flex.AddLast(flex);
                list.AddLast(l);
            }
            else
            {
                this.flex.AddFirst(flex);
                list.AddFirst(l);
            }
        }

        public IEnumerable<T> Peek()
        {
            return list.First?.Value ?? throw new IndexOutOfRangeException();
        }

        public IEnumerable<T> Dequeue()
        {
            var peek = Peek();
            list.RemoveFirst();
            flex.RemoveFirst();
            return peek;
        }
    }
}
