using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Engine
{
    public class ConverterException : Exception
    {
        public ConverterException(Type source, Type target)
            : this(source, target, null)
        {

        }

        public ConverterException(Type source, Type target, Exception? innerException)
            : this($"Could not convert element of type {source.FormattedName()} to {target.FormattedName()}.", innerException)
        {

        }

        public ConverterException(string message) : base(message)
        {
        }

        public ConverterException(string message, Exception? innerException) : base(message, innerException)
        {
        }

        public ConverterException()
        {
        }
    }

    public interface IConverter
    {
        object Convert(object source, Type targetType);
    }
}
