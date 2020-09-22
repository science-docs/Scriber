using System;
using System.Collections.Generic;

namespace Scriber.Engine
{
    public class MergedElementConverter : IConverter
    {
        private readonly IConverter first;
        private readonly List<(Type type, IConverter converter)> converters = new List<(Type, IConverter)>();

        public MergedElementConverter(IConverter first)
        {
            this.first = first ?? throw new ArgumentNullException(nameof(first));
        }

        public void Add(Type converterSourceType, IConverter converter)
        {
            if (converterSourceType is null)
            {
                throw new ArgumentNullException(nameof(converterSourceType));
            }

            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            converters.Add((converterSourceType, converter));
        }

        public object Convert(object source, Type targetType)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (converters.Count == 0)
            {
                return first.Convert(source, targetType);
            }

            var converterList = new List<IConverter>(converters.Count);
            var typeList = new List<Type>(converters.Count);

            converterList.Add(first);

            foreach (var (type, converter) in converters)
            {
                typeList.Add(type);
                converterList.Add(converter);
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
