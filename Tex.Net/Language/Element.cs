using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Tex.Net.Language
{
    public enum ElementType
    {
        Text,
        Paragraph,
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

    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Element
    {
        public ElementType Type { get; set; }
        public LinkedList<Element> Inlines { get; } = new LinkedList<Element>();
        public Element? Parent { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
        public string? Content { get; set; }
        public TextType ContentType { get; set; }
        internal StringBuilder? StringBuilder { get; set; }

        public Element(Element? parent, ElementType type, int index)
        {
            Type = type;
            Parent = parent;
            Index = index;

            if (parent != null)
            {
                parent.Inlines.AddLast(this);
            }
        }

        string DebuggerDisplay
        {
            get
            {
                if (Content != null)
                {
                    return $"{Type} '{Content}'";
                }
                else if (Type == ElementType.Paragraph)
                {
                    return Type.ToString();
                }
                else
                {
                    return $"{Type} Count: {Inlines.Count}";
                }
            }
        }
    }
}
