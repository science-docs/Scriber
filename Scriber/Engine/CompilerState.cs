﻿using Scriber.Engine.Instructions;
using Scriber.Language;
using Scriber.Language.Syntax;
using Scriber.Layout.Document;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace Scriber.Engine
{
    public class CompilerState
    {
        public Context Context { get; }
        public Document Document { get; }
        public CompilerIssueCollection Issues { get; }
        public IFileSystem FileSystem => Context.FileSystem;
        public CommandCollection Commands => Context.Commands;
        public ConverterCollection Converters => Context.Converters;
        public bool Continue { get; private set; } = true;

        public CompilerState(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Document = new Document();
            Issues = new CompilerIssueCollection();
        }

        public void Stop()
        {
            Continue = false;
        }

        public Argument? Execute(SyntaxNode node)
        {
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
