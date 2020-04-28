using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public class ConverterException : Exception
    {
        public ConverterException(Type source, Type target)
            : this(source, target, null)
        {

        }

        public ConverterException(Type source, Type target, Exception? innerException)
            : this($"Could not convert element of type {source.Name} to {target.Name}", innerException)
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

    public interface IElementConverter
    {
        object Convert(object source, Type targetType, CompilerState state);
    }
}
