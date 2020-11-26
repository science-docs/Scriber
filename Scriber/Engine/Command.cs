using Scriber.Language.Syntax;
using System.Collections.Generic;

namespace Scriber.Engine
{
    public delegate object? CommandExecution(SyntaxNode origin, CompilerState state, Argument[] arguments);

    public class Command
    {
        public string Name { get; }
        public CommandExecution Execution { get; }
        public IReadOnlyList<Parameter> Parameters { get; }

        public Command(string name, CommandExecution execution, Parameter[] parameters)
        {
            Name = name;
            Execution = execution;
            Parameters = parameters;
        }
    }
}
