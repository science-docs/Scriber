using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Tex.Net.Language
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
