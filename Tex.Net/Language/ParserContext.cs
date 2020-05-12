using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tex.Net.Language
{
    public class ParserContext
    {
        public bool Newline { get; set; }
        public bool Comment { get; set; }
        public bool BracketBlock { get; set; }
        public Stack<Element> Parents { get; } = new Stack<Element>();

        public List<Element> CurrentLine { get; } = new List<Element>();

        public List<Element> LastLine { get; } = new List<Element>();
    }
}
