using Scriber.Engine.Instructions;
using Scriber.Language;
using System.Linq;

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

        public object? Execute(Element element, object?[] arguments)
        {
            var instruction = EngineInstruction.Create(element);

            try
            {
                return instruction?.Execute(this, arguments);
            }
            catch (CompilerException compilerException)
            {
                Issues.Add(compilerException.Origin ?? element, CompilerIssueType.Error, compilerException.Message, compilerException.InnerException);
            }


            return null;
        }
    }
}
