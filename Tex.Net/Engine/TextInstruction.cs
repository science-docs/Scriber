using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public class TextInstruction : EngineInstruction
    {
        public string Content { get; set; }

        private TextInstruction()
        {

        }

        public static new TextInstruction Create(Element element)
        {
            var text = new TextInstruction
            {
                Content = element.Content
            };
            return text;
        }

        public override object Execute(CompilerState state, object[] arguments)
        {
            // ignore arguments, as text nodes cannot contain children.
            var env = state.Environments.Current;
            return new Layout.Document.Text
            {
                Content = Content,
                Font = env.Font,
                FontSize = env.FontSize.Point
            };
        }
    }
}
