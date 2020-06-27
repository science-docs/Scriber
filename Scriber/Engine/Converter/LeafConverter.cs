using System;
using Scriber.Layout.Document;
using Scriber.Util;

namespace Scriber.Engine.Converter
{
    [CommandArgumentConverter(typeof(Leaf), typeof(Paragraph))]
    class LeafConverter : IElementConverter
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

            if (source is Leaf leaf)
            {
                if (targetType == typeof(Paragraph))
                {
                    var paragraph = new Paragraph();
                    paragraph.Leaves.Add(leaf);
                    return paragraph;
                }
                else
                {
                    throw new ConverterException($"Leaf cannot be converted to object of type {targetType.FormattedName()} by this converter.");
                }
            }
            else
            {
                throw new ConverterException($"Object of type {source.GetType().FormattedName()} cannot be processed by this converter.");
            }
        }
    }
}
