using Scriber.Layout;
using System;
using System.Globalization;

namespace Scriber.Engine.Converter
{
    [CommandArgumentConverter(typeof(string),
        typeof(Enum), typeof(Unit),
        typeof(int), typeof(uint),
        typeof(float), typeof(double),
        typeof(byte), typeof(sbyte),
        typeof(short), typeof(ushort),
        typeof(long), typeof(ulong),
        typeof(decimal), typeof(bool))]
    public class StringConverter : IElementConverter
    {
        public object Convert(object source, Type targetType)
        {
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
            }
            
            throw new ConverterException(source.GetType(), targetType);
        }
    }
}
