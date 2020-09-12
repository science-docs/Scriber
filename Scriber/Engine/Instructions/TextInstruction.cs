using System;
using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class TextInstruction : EngineInstruction
    {
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public TextInstruction(Element origin) : base(origin)
        {
            Content = origin.Content ?? throw new ArgumentException("Content is not allowed to be null", nameof(origin));
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
