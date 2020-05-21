using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Language
{
    public class ParserContext
    {
        public bool Newline { get; set; }
        public bool Comment { get; set; }
        public bool Quotation { get; set; }
        public int Line { get; set; }
        public Stack<Element> Parents { get; } = new Stack<Element>();

        public List<Element> CurrentLine { get; } = new List<Element>();

        public List<Element> LastLine { get; } = new List<Element>();

        public bool InCommand()
        {
            return Parents.Any(e => e.Type == ElementType.Command);
        }

        public Element PeekParent(int i)
        {
            int level = 0;
            Element? last = null;
            foreach (var element in Parents)
            {
                last = element;
                if (level++ == i)
                {
                    return element;
                }
            }
            return last ?? throw new IndexOutOfRangeException();
        }
    }
}
