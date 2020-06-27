using System;
using System.Collections.Generic;

namespace Scriber.Engine
{
    public class MergedElementConverter : IElementConverter
    {
        private readonly IElementConverter first;
        private readonly List<Tuple<Type, IElementConverter>> converters = new List<Tuple<Type, IElementConverter>>();

        public MergedElementConverter(IElementConverter first)
        {
            this.first = first;
        }

        public void Add(Type converterSourceType, IElementConverter converter)
        {
            converters.Add(new Tuple<Type, IElementConverter>(converterSourceType, converter));
        }

        public object Convert(object source, Type targetType)
        {
            if (converters.Count == 0)
            {
                return first.Convert(source, targetType);
            }

            var converterList = new List<IElementConverter>(converters.Count);
            var typeList = new List<Type>(converters.Count);

            converterList.Add(first);

            foreach (var tuple in converters)
            {
                typeList.Add(tuple.Item1);
                converterList.Add(tuple.Item2);
            }

            typeList.Add(targetType);
            var value = source;

            for (int i = 0; i < converterList.Count; i++)
            {
                var converter = converterList[i];
                var type = typeList[i];

                value = converter.Convert(value, type);
            }

            return value;
        }
    }
}
