﻿using System;
using Scriber.Layout.Document;

namespace Scriber.Engine.Converter
{
    [CommandArgumentConverter(typeof(Leaf), typeof(Paragraph))]
    public class LeafConverter : IElementConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ConverterException"/>
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
                    return Paragraph.FromLeaves(leaf);
                }
            }

            throw new ConverterException(source.GetType(), targetType);
        }
    }
}
