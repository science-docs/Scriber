using System;
using System.Reflection;

namespace Scriber.Engine
{
    public class Variable
    {
        public Type Type { get; }
        public string Name { get; }

        private readonly dynamic local;

        public Variable(string name, object documentLocal)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (documentLocal is null)
            {
                throw new ArgumentNullException(nameof(documentLocal));
            }

            Name = name;

            var type = documentLocal.GetType();
            if (!type.IsGenericType)
            {
                throw new ArgumentException("Input value is not a document local", nameof(documentLocal));
            }

            var genericType = type.GetGenericTypeDefinition();
            if (genericType != typeof(DocumentLocal<>))
            {
                throw new ArgumentException("Input value is not a document local", nameof(documentLocal));
            }

            var typeArg = type.GetGenericArguments()[0];
            Type = typeArg;
            local = documentLocal;
        }

        public void Set(Document document, object value)
        {
            local.Set(document, value);
        }

        public static Variable FromPropertyInfo(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<VariableAttribute>();
            if (attribute is null)
            {
                throw new Exception();
            }

            var name = attribute.Name;
            var value = property.GetValue(null);

            if (value is null)
            {
                throw new Exception();
            }

            return new Variable(name, value);
        }
    }
}
