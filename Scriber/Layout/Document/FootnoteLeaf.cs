using Scriber.Text;
using System;
using System.Collections.Generic;

namespace Scriber.Layout.Document
{
    public class FootnoteLeaf : Leaf, ITextLeaf
    {
        public string Content { get; }
        public Paragraph Element { get; }

        public override IEnumerable<Symbol> Symbols => Element.Symbols;

        public FootnoteLeaf(string content, Paragraph value) : this(content, value, true)
        {
        }

        private FootnoteLeaf(string content, Paragraph value, bool addPrefix)
        {
            Tag = "span";
            Classes.Add("footnote-reference");
            Content = content;
            Element = value ?? throw new ArgumentNullException(nameof(value));

            if (addPrefix)
            {
                var prefix = new TextLeaf(content) { Tag = Tag };
                prefix.Classes.Add("footnote-reference");
                value.Leaves.Insert(0, prefix);
            }
        }

        public override IEnumerable<AbstractElement> ChildElements()
        {
            yield return Element;
        }

        protected override LineNode[] GetNodesOverride()
        {
            Element.Parent = Parent;
            return LineNodeTransformer.Create(this).ToArray();
        }

        protected override AbstractElement CloneInternal()
        {
            return new FootnoteLeaf(Content, Element.Clone(), false);
        }

        
    }
}
