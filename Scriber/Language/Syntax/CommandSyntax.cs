using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public class CommandSyntax : SyntaxNode
    {
        public int Arity => Arguments?.Children?.Count ?? 0;
        public NameSyntax? Name { get; set; }
        public ListSyntax<ArgumentSyntax>? Arguments { get; set; }
        public ListSyntax? EnvironmentBlock { get; set; }

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Name != null)
            {
                yield return Name;
            }
            if (Arguments != null)
            {
                yield return Arguments;
            }
            if (EnvironmentBlock != null)
            {
                yield return EnvironmentBlock;
            }
        }
    }
}
