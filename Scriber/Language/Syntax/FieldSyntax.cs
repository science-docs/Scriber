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
        public ListSyntax<ListSyntax>? Value
        {
            get => value;
            set => this.value = SetParent(this.value, value);
        }

        private NameSyntax? name;
        private ListSyntax<ListSyntax>? value;

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Name != null)
            {
                yield return Name;
            }
            if (Value != null)
            {
                yield return Value;
            }
        }
    }
}
