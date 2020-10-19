using Scriber.Layout;
using System;
using System.Globalization;

namespace Scriber.Engine.Converter
{
    [Converter(typeof(string),
        typeof(Enum), typeof(Unit),
        typeof(int), typeof(uint),
        typeof(float), typeof(double),
        typeof(byte), typeof(sbyte),
        typeof(short), typeof(ushort),
        typeof(long), typeof(ulong),
        typeof(decimal), typeof(bool),
        typeof(Index), typeof(Range))]
    public class StringConverter : IConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ConverterException"/>
        /// <exception cref="FormatException"/>
        /// <exception cref="OverflowException"/>
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

            if (source is string value)
            {
                if (targetType.IsEnum)
                {
                    if (long.TryParse(value, out long enumValue))
                    {
                        return Enum.ToObject(targetType, enumValue);
                    }
                    else
                    {
                        return Enum.Parse(targetType, value, true);
                    }
                }
                else if (targetType == typeof(int))
                {
                    return int.Parse(value);
                }
                else if (targetType == typeof(uint))
                {
                    return uint.Parse(value);
                }
                else if (targetType == typeof(float))
                {
                    return float.Parse(value, CultureInfo.InvariantCulture);
                }
                else if (targetType == typeof(double))
                {
                    return double.Parse(value, CultureInfo.InvariantCulture);
                }
                else if (targetType == typeof(byte))
                {
                    return byte.Parse(value);
                }
                else if (targetType == typeof(sbyte))
                {
                    return sbyte.Parse(value);
                }
                else if (targetType == typeof(short))
                {
                    return short.Parse(value);
                }
                else if (targetType == typeof(ushort))
                {
                    return ushort.Parse(value);
                }
                else if (targetType == typeof(long))
                {
                    return long.Parse(value);
                }
                else if (targetType == typeof(ulong))
                {
                    return ulong.Parse(value);
                }
                else if (targetType == typeof(decimal))
                {
                    return decimal.Parse(value, CultureInfo.InvariantCulture);
                }
                else if (targetType == typeof(bool))
                {
                    return bool.Parse(value);
                }
                else if (targetType == typeof(Unit) && Unit.TryParse(value, out var unit))
                {
                    return unit;
                }
                else if (targetType == typeof(Index))
                {
                    return ParseIndex(value);
                }
                else if (targetType == typeof(Range))
                {
                    var split = value.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length == 1)
                    {
                        var index = ParseIndex(split[0]);
                        return index..index;
                    }
                    else if (split.Length == 2)
                    {
                        return ParseIndex(split[0])..ParseIndex(split[1]);
                    }
                    else
                    {
                        throw new ConverterException($"Cannot convert '{value}' to range.");
                    }
                }
            }
            
            throw new ConverterException(source.GetType(), targetType);
        }

        public static T Convert<T>(string input)
        {
            var converter = new StringConverter();
            return (T)converter.Convert(input, typeof(T));
        }

        private Index ParseIndex(string value)
        {
            if (value.StartsWith("^"))
            {
                return new Index(int.Parse(value.Substring(1)), true);
            }
            else
            {
                return new Index(int.Parse(value));
            }
        }
    }
}
