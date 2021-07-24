using System;
using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public class ObjectSyntax : SyntaxNode
    {
        public ListSyntax<FieldSyntax> Fields
        {
            get => fields ?? throw new NullReferenceException();
            set  
            {
                fields = value ?? throw new NullReferenceException();
                fields.Parent = this;
            }
        }

        private ListSyntax<FieldSyntax>? fields;

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            yield return Fields;
        }
    }
}
