using Scriber.Drawing;
using Scriber.Layout;
using Scriber.Maths;
using System;

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
        typeof(Index), typeof(Range),
        typeof(Thickness), typeof(Color))]
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
                    return MathParser.Evaluate(value).AsInt();
                }
                else if (targetType == typeof(uint))
                {
                    return MathParser.Evaluate(value).AsUInt();
                }
                else if (targetType == typeof(float))
                {
                    return MathParser.Evaluate(value).AsSingle();
                }
                else if (targetType == typeof(double))
                {
                    return MathParser.Evaluate(value).AsDouble();
                }
                else if (targetType == typeof(byte))
                {
                    return MathParser.Evaluate(value).AsByte();
                }
                else if (targetType == typeof(sbyte))
                {
                    return MathParser.Evaluate(value).AsSByte();
                }
                else if (targetType == typeof(short))
                {
                    return MathParser.Evaluate(value).AsShort();
                }
                else if (targetType == typeof(ushort))
                {
                    return MathParser.Evaluate(value).AsUShort();
                }
                else if (targetType == typeof(long))
                {
                    return MathParser.Evaluate(value).AsLong();
                }
                else if (targetType == typeof(ulong))
                {
                    return MathParser.Evaluate(value).AsULong();
                }
                else if (targetType == typeof(decimal))
                {
                    return MathParser.Evaluate(value).AsDecimal();
                }
                else if (targetType == typeof(bool))
                {
                    return bool.Parse(value);
                }
                else if (targetType == typeof(Unit))
                {
                    if (Unit.TryParse(value, out var unit))
                    {
                        return unit;
                    }
                }
                else if (targetType == typeof(Thickness))
                {
                    if (Thickness.TryParse(value, out var thickness))
                    {
                        return thickness;
                    }
                }
                else if (targetType == typeof(Color))
                {
                    if (Color.TryParse(value, out var color))
                    {
                        return color;
                    }
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

        private Index ParseIndex(string value)
        {
            if (value.StartsWith("^"))
            {
                return new Index(int.Parse(value[1..]), true);
            }
            else
            {
                return new Index(int.Parse(value));
            }
        }
    }
}
