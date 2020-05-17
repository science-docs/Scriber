using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Layout.Document;

namespace Scriber.Engine.Converter
{
    [CommandArgumentConverter(typeof(Paragraph), typeof(string), typeof(IEnumerable<Leaf>))]
    public class ParagraphConverter : IElementConverter
    {
        public object Convert(object source, Type targetType, CompilerState state)
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
                    StringBuilder sb = new StringBuilder();

                    foreach (var leaf in paragraph.Leaves)
                    {
                        if (leaf is ITextLeaf text)
                        {
                            sb.Append(text.Content);
                        }
                        else
                        {
                            throw new ConverterException("Could not convert paragraph to string. Paragraph contains non string content");
                        }
                    }

                    return sb.ToString();
                }
                else
                {
                    throw new ConverterException($"Paragraph cannot be converted to object of type {targetType.Name} by this converter");
                }
            }
            else
            {
                throw new ConverterException($"Object of type {source.GetType().Name} cannot be processed by this converter");
            }

        }
    }
}
