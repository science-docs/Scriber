using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Layout
{
    public sealed class LayoutException : Exception
    {
        public LayoutException(string? message) : base(message)
        {
        }

        public LayoutException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public LayoutException()
        {
        }
    }
}
