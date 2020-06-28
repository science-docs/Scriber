using Scriber.Language;
using System.Collections.Generic;
using System.Reflection;

namespace Scriber.Engine
{
    public delegate object? CommandExecution(Element origin, CompilerState state, Argument[] arguments);

    public class Command
    {
        public string Name { get; }
        public string? RequiredEnvironment { get; }
        public CommandExecution Execution { get; }
        public IReadOnlyList<ParameterInfo> Parameters { get; }

        public Command(string name, string? requiredEnvironment, CommandExecution execution, ParameterInfo[] parameters)
        {
            Name = name;
            RequiredEnvironment = requiredEnvironment;
            Execution = execution;
            Parameters = parameters;
        }
    }
}
