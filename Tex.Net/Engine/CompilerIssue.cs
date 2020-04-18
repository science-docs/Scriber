using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public class CompilerIssue
    {
        public int Index { get; set; }
        public string Message { get; set; }
        public Exception InnerException { get; set; }
    }
}
