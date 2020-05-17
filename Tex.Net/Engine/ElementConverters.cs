using System;
using System.Collections.Generic;

namespace Tex.Net.Engine
{
    public static class ElementConverters
    {
        private static readonly Dictionary<Tuple<Type, Type>, IElementConverter> converters = new Dictionary<Tuple<Type, Type>, IElementConverter>();

        public static void Add(IElementConverter converter, Type source, params Type[] targets)
        {
            foreach (var target in targets)
            {
                converters[new Tuple<Type, Type>(source, target)] = converter;
            }
        }

        public static IElementConverter? Find(Type source, Type target)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            converters.TryGetValue(new Tuple<Type, Type>(source, target), out var converter);
            return converter;
        }

        public static object? Convert(object? source, Type target)
        {
            if (source == null)
            {
                return null;
            }

            var type = source.GetType();
            var converter = Find(type, target);

            if (converter != null)
            {
                return converter.Convert(source, target, new CompilerState());
            }
            else
            {
                return null;
            }
        }
    }
}
