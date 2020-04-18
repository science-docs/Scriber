using System.Collections.Generic;
using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public class CompilerState
    {
        public Document Document { get; private set; }

        public List<CompilerIssue> Issues { get; } = new List<CompilerIssue>();

        public void Execute(Element element)
        {
            var instruction = EngineInstruction.Create(element);

            var result = instruction?.Execute(this);

            switch (result)
            {
                case Document doc:
                    Document = doc;
                    break;
            }
        }

        
    }
}
