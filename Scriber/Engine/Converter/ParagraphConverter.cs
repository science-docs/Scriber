using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Layout.Document;
using Scriber.Util;

namespace Scriber.Engine.Converter
{
    [CommandArgumentConverter(typeof(Paragraph), typeof(string), typeof(IEnumerable<Leaf>))]
    public class ParagraphConverter : IElementConverter
    {
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

            if (source is Paragraph paragraph)
            {
                if (targetType == typeof(IEnumerable<Leaf>))
                {
                    return paragraph.Leaves;
                }
                else if (targetType == typeof(string))
                {
                    return ConvertToString(paragraph);
                }
                else if (targetType.IsEnum)
                {
                    var text = ConvertToString(paragraph);
                    if (long.TryParse(text, out long enumValue))
                    {
                        return Enum.ToObject(targetType, enumValue);
                    }
                    else
                    {
                        return Enum.Parse(targetType, text, true);
                    }
                }
                else
                {
                    throw new ConverterException($"Paragraph cannot be converted to object of type {targetType.FormattedName()} by this converter.");
                }
            }
            else
            {
                throw new ConverterException($"Object of type {source.GetType().FormattedName()} cannot be processed by this converter.");
            }

        }

        private static string ConvertToString(Paragraph paragraph)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var leaf in paragraph.Leaves)
            {
                if (leaf is ITextLeaf text)
                {
                    sb.Append(text.Content);
                }
                else
                {
                    throw new ConverterException("Could not convert paragraph to string. Paragraph contains non string content.");
                }
            }

            return sb.ToString();
        }
    }
}
