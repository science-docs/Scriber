using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Language
{
    public enum ElementType
    {
        Text,
        Block,
        Math,
        Command,
        Comment
    }

    public enum TextType
    {
        Normal,
        Subscript,
        Superscript
    }

    public class Element
    {
        public ElementType Type { get; set; }
        public Element Inline { get; set; }
        public Element Parent { get; set; }
        public Element NextSibling { get; set; }
        public Element PreviousSibling { get; set; }
        public Element LastSibling => NextSibling == null ? this : NextSibling.LastSibling;
        public int Index { get; set; }
        public int Length { get; set; }
        public string Content { get; set; }
        public TextType ContentType { get; set; }
        internal StringBuilder StringBuilder { get; set; }
        public List<Element> CommandArguments { get; } = new List<Element>();

        public Element(Element parent, ElementType type, int index)
        {
            Type = type;
            Parent = parent;
            Index = index;

            if (parent != null)
            {
                if (parent.Inline != null)
                {
                    parent.Inline.AddSibling(this);
                }
                else
                {
                    parent.Inline = this;
                }
            }
        }

        public void SetNextSibling(Element element)
        {
            element.PreviousSibling = this;
            NextSibling = element;
        }

        public void AddSibling(Element element)
        {
            var last = LastSibling;
            element.PreviousSibling = last;
            last.NextSibling = element;
        }

        public override string ToString()
        {
            if (Content != null)
            {
                return $"{Type} '{Content}'";
            }
            else
            {
                return $"{Type} Count: {InlineCount()}";
            }
        }

        private int InlineCount()
        {
            int i = 0;
            var inline = Inline;
            while (inline != null)
            {
                inline = inline.NextSibling;
                i++;
            }
            return i;
        }
    }
}
