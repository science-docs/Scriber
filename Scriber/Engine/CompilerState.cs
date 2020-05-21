using Scriber.Engine.Instructions;
using Scriber.Language;
using Scriber.Logging;

namespace Scriber.Engine
{
    public class CompilerState
    {
        public Document Document { get; }
        public BlockTree Blocks { get; }
        public CompilerIssueCollection Issues { get; } = new CompilerIssueCollection();

        public CompilerState()
        {
            Document = new Document();
            Blocks = new BlockTree();
        }


        public CompilerState(Logger logger) : this()
        {

        }

        public object? Execute(Element element, object?[] arguments)
        {
            var instruction = EngineInstruction.Create(element);

            var result = instruction?.Execute(this, arguments);

            return result;
        }
    }
}
