using System;
using System.Collections.Generic;

namespace Scriber.Language.Syntax
{
    public class CommandSyntax : SyntaxNode
    {
        public int Arity => Arguments.Count;

        public NameSyntax Name
        {
            get => name ?? throw new NullReferenceException();
            set => name = SetParent(name, value ?? throw new NullReferenceException()); 
        }

        public ListSyntax<ArgumentSyntax> Arguments
        {
            get => arguments ?? throw new NullReferenceException();
            set => arguments = SetParent(arguments, value ?? throw new NullReferenceException());
        }

        public ListSyntax? EnvironmentBlock
        {
            get => environmentBlock;
            set => environmentBlock = SetParent(environmentBlock, value);
        }

        private NameSyntax? name;
        private ListSyntax<ArgumentSyntax>? arguments;
        private ListSyntax? environmentBlock;

        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (name != null)
            {
                yield return name;
            }
            if (arguments != null)
            {
                yield return arguments;
            }
            if (EnvironmentBlock != null)
            {
                yield return EnvironmentBlock;
            }
        }
    }
}
