using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public class TextInstruction : EngineInstruction
    {
        public string Content { get; set; }

        private TextInstruction(string content)
        {
            Content = content;
        }

        public static new TextInstruction Create(Element element)
        {
            var text = new TextInstruction(element.Content ?? throw new ArgumentNullException(nameof(element.Content)));
            return text;
        }

        public override object Execute(CompilerState state, object[] arguments)
        {
            // ignore arguments, as text nodes cannot contain children.
            return new Layout.Document.TextLeaf
            {
                Content = Content
            };
        }
    }
}
