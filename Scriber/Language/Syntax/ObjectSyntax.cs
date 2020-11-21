using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public class ObjectSyntax : SyntaxNode
    {
        public ListSyntax<FieldSyntax>? Fields { get; set; }

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Fields != null)
            {
                yield return Fields;
            }
        }
    }
}
