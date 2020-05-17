using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Scriber.Language
{
    public class ParserException : Exception
    {
        public ParserException()
        {
        }

        public ParserException(string? message) : base(message)
        {
        }

        public ParserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
