using System.Collections.Generic;
using Scriber.Engine.Instructions;
using Scriber.Language;

namespace Scriber.Engine
{
    public class CompilerState
    {
        public Document Document { get; }
        public BlockTree Blocks { get; }
        public List<CompilerIssue> Issues { get; } = new List<CompilerIssue>();

        public CompilerState()
        {
            Document = new Document();
            Blocks = new BlockTree();
        }

        public object? Execute(Element element, object[] arguments)
        {
            var instruction = EngineInstruction.Create(element);

            var result = instruction?.Execute(this, arguments);

            return result;
        }

        
    }
}
