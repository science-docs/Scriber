using Scriber.Language;
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

        private static void Flatten(Argument argument, List<Argument> list)
        {
            if (argument.Value is IEnumerable<Argument> arguments)
            {
                foreach (var arg in arguments)
                {
                    Flatten(arg, list);
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
