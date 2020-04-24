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
        public LinkedList<Element> Inlines { get; } = new LinkedList<Element>();
        public Element Parent { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
        public string Content { get; set; }
        public TextType ContentType { get; set; }
        internal StringBuilder StringBuilder { get; set; }

        public Element(Element parent, ElementType type, int index)
        {
            Type = type;
            Parent = parent;
            Index = index;

            if (parent != null)
            {
                parent.Inlines.AddLast(this);
            }
        }

        public override string ToString()
        {
            if (Content != null)
            {
                return $"{Type} '{Content}'";
            }
            else
            {
                return $"{Type} Count: {Inlines.Count}";
            }
        }
    }
}
