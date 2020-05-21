using Scriber.Language;
using System;

namespace Scriber.Engine
{
    public class CompilerException : Exception
    {
        public Element? Origin { get; set; }

        public CompilerException(Element? origin) : this(origin, null, null)
        {
        }

        public CompilerException(Element? origin, string? message) : this(origin, message, null)
        {
        }

        public CompilerException(Element? origin, string? message, Exception? innerException) : this(message, innerException)
        {
            Origin = origin;
        }

        public CompilerException()
        {
        }

        public CompilerException(string? message) : base(message)
        {
        }

        public CompilerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
