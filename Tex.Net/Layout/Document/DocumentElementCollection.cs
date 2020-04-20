using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Layout.Document
{
    public class DocumentElementCollection<T> : IList<T> where T : DocumentElement
    {
        private readonly List<T> list = new List<T>();

        public T this[int index]
        { 
            get => list[index]; 
            set
            {
                if (index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }

                list[index].Parent = null;
                list[index] = value ?? throw new ArgumentNullException(nameof(value));
                value.Parent = Parent;
            }
        }

        public int Count => list.Count;

        public DocumentElement Parent { get; }

        public bool IsReadOnly => false;

        public DocumentElementCollection(DocumentElement parent)
        {
            Parent = parent;
        }

        public void Add(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.Parent = Parent;
            list.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            foreach (var item in list)
            {
                item.Parent = null;
            }

            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.Parent = Parent;
            list.Insert(index, item);
        }

        public bool Remove(T item)
        {
            var result = list.Remove(item);
            if (result)
            {
                item.Parent = null;
            }
            return result;
        }

        public void RemoveAt(int index)
        {
            list[index].Parent = null;
            list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
