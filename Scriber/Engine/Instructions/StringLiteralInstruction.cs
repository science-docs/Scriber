using Scriber.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Engine.Instructions
{
    public class StringLiteralInstruction : EngineInstruction<StringLiteralSyntax>
    {
        public override object? Evaluate(CompilerState state, StringLiteralSyntax node)
        {
            return new Layout.Document.TextLeaf
            {
                Content = node.Content
            };
        }
    }
}
