using System;
using System.Collections;
using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public class ListSyntax : ListSyntax<SyntaxNode>
    {

    }

    public class ListSyntax<T> : SyntaxNode, IList<T> where T : SyntaxNode
    {
        private readonly List<T> items = new List<T>();

        public T this[int index] 
        {
            get => items[index];
            set => items[index] = value ?? throw new ArgumentNullException(nameof(value)); 
        }

        public T this[Index index]
        {
            get => items[index];
            set => items[index] = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (item != null)
            {
                item.Parent = this;
                items.Add(item);
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                Add(item);
            }
        }

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            return items;
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            items.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
