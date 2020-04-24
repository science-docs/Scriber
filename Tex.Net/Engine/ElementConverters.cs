using System;
using System.Collections.Generic;
using System.Text;

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

        public static IElementConverter Find(Type source, Type target)
        {
            converters.TryGetValue(new Tuple<Type, Type>(source, target), out var converter);
            return converter;
        }
    }
}
