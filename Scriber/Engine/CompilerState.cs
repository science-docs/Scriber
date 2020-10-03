using Scriber.Engine.Instructions;
using Scriber.Language;
using System;
using System.IO.Abstractions;

namespace Scriber.Engine
{
    public class CompilerState
    {
        public Context Context { get; }
        public Document Document { get; }
        public BlockStack Blocks { get; }
        public CompilerIssueCollection Issues { get; }

        public IFileSystem FileSystem => Context.FileSystem;
        public CommandCollection Commands => Context.Commands;
        public ConverterCollection Converters => Context.Converters;

        public CompilerState(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Document = new Document();
            Blocks = new BlockStack();
            Issues = new CompilerIssueCollection();
        }

        public Argument? Execute(Element element, Argument[] arguments)
        {
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
