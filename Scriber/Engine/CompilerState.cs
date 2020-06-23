using Scriber.Engine.Instructions;
using Scriber.Language;
using System.Linq;

namespace Scriber.Engine
{
    public class CompilerState
    {
        public Document Document { get; }
        public BlockTree Blocks { get; }
        public Element CurrentElement { get; private set; }
        public CompilerIssueCollection Issues { get; } = new CompilerIssueCollection();

        public CompilerState()
        {
            CurrentElement = new Element(null, ElementType.Null, 0, 0);
            Document = new Document();
            Blocks = new BlockTree();
        }

        public Argument? Execute(Element element, Argument[] arguments)
        {
            CurrentElement = element;
            var instruction = EngineInstruction.Create(element);

            try
            {
                var result = instruction?.Execute(this, arguments);

                if (result != null)
                {
                    return new Argument(element, result);
                }
            }
            catch (CompilerException compilerException)
            {
                Issues.Add(compilerException.Origin ?? element, CompilerIssueType.Error, compilerException.Message, compilerException.InnerException);
            }


            return null;
        }
    }
}
