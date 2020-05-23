using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Scriber.Language
{
    public enum ElementType
    {
        Text,
        Quotation,
        Paragraph,
        ObjectCreation,
        ObjectField,
        ObjectArray,
        // Default Blocks
        Block,
        // Blocks initialized with curly braces
        ExplicitBlock,
        Math,
        Command,
        Environment,
        Comment,
        /// <summary>
        /// For when a command parameter is actually a 'null' literal
        /// </summary>
        Null
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
        public LinkedList<Element> Children { get; } = new LinkedList<Element>();
        public Element? Parent { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
        public int Line { get; set; }
        public string? Content { get; set; }
        public TextType ContentType { get; set; }
        internal StringBuilder? StringBuilder { get; set; }

        public Element(Element? parent, ElementType type, int index, int line)
        {
            Type = type;
            Parent = parent;
            Index = index;
            Line = line;

            if (parent != null)
            {
                parent.Children.AddLast(this);
            }
        }

        public void Detach()
        {
            Parent?.Children.Remove(this);
            Parent = null;
        }

        public void Siblings(out Element? previous, out Element? next)
        {
            if (Parent == null)
            {
                previous = null;
                next = null;
            }
            else
            {
                var node = Parent!.Children.Find(this)!;
                previous = node.Previous?.Value;
                next = node.Next?.Value;
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
                else
                {
                    return $"{Type} Count: {Children.Count}";
                }
            }
        }
    }
}
