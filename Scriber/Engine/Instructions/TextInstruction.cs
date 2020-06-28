using System;
using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class TextInstruction : EngineInstruction
    {
        public string Content { get; set; }

        public TextInstruction(Element origin) : base(origin)
        {
            Content = origin.Content ?? throw new ArgumentNullException(nameof(origin.Content));
        }

        public override object Execute(CompilerState state, Argument[] arguments)
        {
            return new Layout.Document.TextLeaf
            {
                Content = Content
            };
        }
    }
}
