using Scriber.Engine.Instructions;
using Scriber.Language.Syntax;
using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;

namespace Scriber.Engine
{
    public class CompilerState
    {
        public Context Context { get; }
        public Document Document { get; }
        public CompilerIssueCollection Issues { get; }
        public bool Success => Continue && !Issues.Any(e => e.Type == CompilerIssueType.Error);
        public IFileSystem FileSystem => Context.FileSystem;
        public CommandCollection Commands => Context.Commands;
        public ConverterCollection Converters => Context.Converters;
        public bool Continue => continued && !cancellationToken.IsCancellationRequested;

        private bool continued = true;
        private readonly CancellationToken cancellationToken;


        public CompilerState(Context context, CancellationToken cancellationToken)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Document = new Document();
            Issues = new CompilerIssueCollection();
            this.cancellationToken = cancellationToken;
        }

        public void Stop()
        {
            continued = false;
        }

        public Argument? Execute(SyntaxNode node)
        {
            if (!Continue)
            {
                return null;
            }

            try
            {
                var result = EngineInstruction.Evaluate(this, node);

                if (result != null)
                {
                    return new Argument(node, result);
                }
            }
            catch (CompilerException compilerException)
            {
                Issues.Add(compilerException.Origin ?? node, CompilerIssueType.Error, compilerException.Message, compilerException.InnerException);
            
                if (Context.FailOnError)
                {
                    // Instead of propagating the exception, we just stop.
                    Stop();
                }
            }


            return null;
        }
    }
}
