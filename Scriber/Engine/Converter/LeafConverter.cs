using System;
using Scriber.Layout.Document;

namespace Scriber.Engine.Converter
{
    [CommandArgumentConverter(typeof(Leaf), typeof(Paragraph))]
    public class LeafConverter : IElementConverter
    {
        public object Convert(object source, Type targetType)
        {
            if (source is Leaf leaf)
            {
                if (targetType == typeof(Paragraph))
                {
                    return Paragraph.FromLeaves(leaf);
                }
            }

            throw new ConverterException(source.GetType(), targetType);
        }
    }
}
