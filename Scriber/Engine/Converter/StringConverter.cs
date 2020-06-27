using Scriber.Layout;
using System;

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
                else if (targetType == typeof(bool))
                {
                    return bool.Parse(value);
                }
                
            }
            
            throw new ConverterException(source.GetType(), targetType);
        }
    }
}
