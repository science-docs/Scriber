using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public class ListSyntax : ListSyntax<SyntaxNode>
    {

    }

    public class ListSyntax<T> : SyntaxNode where T : SyntaxNode
    {
        public List<T> Children { get; } = new List<T>();

        public void Add(T item)
        {
            if (item != null)
            {
                item.Parent = this;
            }
            Children.Add(item);
        }

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            return Children;
        }
    }
}
