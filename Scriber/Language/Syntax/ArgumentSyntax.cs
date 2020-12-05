using System;
using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public class ArgumentSyntax : SyntaxNode
    {
        public int Position { get; set; }
        public NameSyntax? Name
        {
            get => name;
            set => name = SetParent(name, value);
        }
        public ListSyntax Content
        {
            get => content ?? throw new NullReferenceException(nameof(Content));
            set => content = SetParent(content, value);
        }

        private NameSyntax? name;
        private ListSyntax? content;

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Name != null)
            {
                yield return Name;
            }
            if (Content != null)
            {
                yield return Content;
            }
        }
    }
}
