using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Language
{
    public class ParserContext
    {
        public bool Newline { get; set; }
        public bool Comment { get; set; }
        public Stack<Element> Parents { get; } = new Stack<Element>();
    }
}
