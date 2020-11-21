using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public abstract class SyntaxNode
    {
        public TextSpan Span { get; set; }
        public SyntaxNode? Parent { get; set; }
        public Resource? Resource
        {
            get
            {
                if (resource != null)
                {
                    return resource;
                }
                if (Parent != null)
                {
                    return Parent.Resource;
                }
                return null;
            }
            set
            {
                resource = value;
            }
        }

        private Resource? resource;

        protected T SetParent<T>(T oldNode, T newNode) where T : SyntaxNode?
        {
            if (oldNode != null)
            {
                oldNode.Parent = null;
            }
            if (newNode != null)
            {
                newNode.Parent = this;
            }
            return newNode;
        }

        public virtual IEnumerable<SyntaxNode> ChildNodes()
        {
            yield break;
        }

        public IEnumerable<SyntaxNode> Ancestors()
        {
            return GetAncestors(false);
        }

        public IEnumerable<SyntaxNode> AncestorsAndSelf()
        {
            return GetAncestors(true);
        }

        private List<SyntaxNode> GetAncestors(bool includeSelf)
        {
            var ancestors = new List<SyntaxNode>();
            if (includeSelf)
            {
                ancestors.Add(this);
            }
            var parent = Parent;

            while (parent != null)
            {
                ancestors.Add(parent);
                parent = parent.Parent;
            }

            return ancestors;
        }

        public IEnumerable<SyntaxNode> DescendantNodes()
        {
            return GetDescendantNodes(false);
        }

        public IEnumerable<SyntaxNode> DescendantNodesAndSelf()
        {
            return GetDescendantNodes(true);
        }

        private IEnumerable<SyntaxNode> GetDescendantNodes(bool includeSelf)
        {
            var descendants = new List<SyntaxNode>();

            if (includeSelf)
            {
                descendants.Add(this);
            }

            foreach (var child in ChildNodes())
            {
                if (!includeSelf)
                {
                    descendants.Add(child);
                }
                descendants.AddRange(GetDescendantNodes(includeSelf));
            }

            return descendants;
        }
    }
}
