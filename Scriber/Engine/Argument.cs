using Scriber.Language;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scriber.Engine
{
    public class Argument
    {
        public object? Value { get; }
        public Element Source { get; }

        public Argument(Element source, object? value)
        {
            Source = source;
            Value = value;
        }

        public Argument[] Flatten()
        {
            var list = new List<Argument>();
            Flatten(this, list);
            return list.ToArray();
        }

        public static bool IsArgumentType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Argument<>);
        }

        public static bool IsArgumentType(Type type, out Type genericType)
        {
            if (IsArgumentType(type))
            {
                genericType = type.GenericTypeArguments[0];
                return true;
            }
            else
            {
                genericType = type;
                return false;
            }
        }

        private static void Flatten(Argument argument, List<Argument> list)
        {
            if (argument.Value is IEnumerable<Argument> arguments)
            {
                foreach (var arg in arguments)
                {
                    Flatten(arg, list);
                }
            }
            else if (argument.Value is IEnumerable elements)
            {
                foreach (var element in elements)
                {
                    Flatten(new Argument(argument.Source, element), list);
                }
            }
            else
            {
                list.Add(argument);
            }
        }
    }

    public class Argument<T> : Argument
    {
        public new T Value { get; }

        public Argument(Element source, T value) : base(source, value)
        {
            Value = value;
        }
    }
}
