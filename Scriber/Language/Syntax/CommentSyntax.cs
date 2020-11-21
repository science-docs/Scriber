using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Language.Syntax
{
    public class CommentSyntax : SyntaxNode
    {
        public string Comment { get; set; } = string.Empty;
    }
}
