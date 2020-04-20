using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net
{
    public class DocumentVariable : IEnumerable<KeyValuePair<string, DocumentVariable>>
    {
        private readonly Dictionary<string, DocumentVariable> objects = new Dictionary<string, DocumentVariable>();

        public DocumentVariable this[string name]
        {
            get => objects.TryGetValue(name, out var obj) ? obj : objects[name] = new DocumentVariable();
            set => objects[name] = value;
        }

        private object value;

        public DocumentVariable()
        {

        }

        public DocumentVariable(object value)
        {
            this.value = value;
        }

        public T GetValue<T>()
        {
            if (value is T item)
            {
                return item;
            }

            return default;
        }

        public void SetValue(object value)
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
