using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net
{
    public class DocumentObject : IEnumerable<KeyValuePair<string, DocumentObject>>
    {
        private readonly Dictionary<string, DocumentObject> objects = new Dictionary<string, DocumentObject>();

        public DocumentObject this[string name]
        {
            get => objects.TryGetValue(name, out var obj) ? obj : objects[name] = new DocumentObject();
            set => objects[name] = value;
        }

        public string Value { get; set; }

        public DocumentObject()
        {

        }

        public DocumentObject(string value)
        {
            Value = value;
        }

        public static implicit operator string(DocumentObject documentObject) => documentObject.Value;

        public static implicit operator DocumentObject(string value) => new DocumentObject(value);

        public IEnumerator<KeyValuePair<string, DocumentObject>> GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return objects.GetEnumerator();
        }
    }
}
