using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Layout.Document;

namespace Scriber.Engine.Converter
{
    [CommandArgumentConverter(typeof(Paragraph), typeof(string), typeof(IEnumerable<Leaf>))]
    public class ParagraphConverter : IElementConverter
    {
        public object Convert(object source, Type targetType)
        {
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
            }

            throw new ConverterException(source.GetType(), targetType);
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
                    throw new ConverterException("Could not convert paragraph to string. Paragraph contains non-string content.");
                }
            }

            return sb.ToString();
        }
    }
}
