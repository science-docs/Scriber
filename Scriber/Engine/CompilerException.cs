using Scriber.Language;
using Scriber.Language.Syntax;
using System;

namespace Scriber.Engine
{
    public class CompilerException : Exception
    {
        public SyntaxNode? Origin { get; set; }

        public CompilerException(SyntaxNode? origin) : this(origin, null, null)
        {
        }

        public CompilerException(SyntaxNode? origin, string? message) : this(origin, message, null)
        {
        }

        public CompilerException(SyntaxNode? origin, string? message, Exception? innerException) : this(message, innerException)
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
