using System;
using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public class FieldSyntax : SyntaxNode
    {
        public NameSyntax? Name
        {
            get => name;
            set => name = SetParent(name, value);
        }
        public ListSyntax Value
        {
            get => value ?? throw new NullReferenceException();
            set => this.value = SetParent(this.value, value ?? throw new NullReferenceException());
        }

        private NameSyntax? name;
        private ListSyntax? value;

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Name != null)
            {
                yield return Name;
            }
            yield return Value;
        }
    }
}
