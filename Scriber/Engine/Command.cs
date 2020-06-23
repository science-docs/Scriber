using Scriber.Language;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Scriber.Engine
{
    public delegate object? CommandExecution(Element origin, CompilerState state, Argument[] arguments);

    public class Command
    {
        public string Name { get; set; }
        public string? RequiredEnvironment { get; set; }
        public CommandExecution Execution { get; set; }
        public ReadOnlyCollection<ParameterInfo> Parameters { get; set; }

        public Command(string name, string? requiredEnvironment, CommandExecution execution, ParameterInfo[] parameters)
        {
            Name = name;
            RequiredEnvironment = requiredEnvironment;
            Execution = execution;
            Parameters = new ReadOnlyCollection<ParameterInfo>(parameters);
        }
    }
}
