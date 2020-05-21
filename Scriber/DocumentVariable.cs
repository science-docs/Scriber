using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Scriber
{
    public class DocumentVariable : IEnumerable<KeyValuePair<string, DocumentVariable>>
    {
        private readonly Dictionary<string, DocumentVariable> objects = new Dictionary<string, DocumentVariable>();

        public DocumentVariable this[string name]
        {
            get => objects.TryGetValue(name, out var obj) ? obj : objects[name] = new DocumentVariable();
            set => objects[name] = value;
        }

        public DocumentVariable this[params string[] names]
        {
            get => names.Length == 1 ? this[names[0]] : this[names[0]][names[1..^0]];
            set
            {
                if (names.Length == 1)
                {
                    this[names[0]] = value;
                }
                else
                {
                    this[names[0]][names[1..^0]] = value;
                }
            }
        }

        private object? value;

        public DocumentVariable()
        {

        }

        public DocumentVariable(object? value)
        {
            this.value = value;
        }

        [return: MaybeNull]
        public T GetValue<T>()
        {
            if (value is T item)
            {
                return item;
            }

            return default;
        }

        public bool OfType<T>()
        {
            if (value is T)
            {
                return true;
            }
            return false;
        }

        public void SetValue(object? value)
        {
            this.value = value;
        }


        public static implicit operator DocumentVariable(string value) => new DocumentVariable(value);

        public IEnumerator<KeyValuePair<string, DocumentVariable>> GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return objects.GetEnumerator();
        }
    }
}
