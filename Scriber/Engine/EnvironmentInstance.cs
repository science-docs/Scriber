using Scriber.Language;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Scriber.Engine
{
    public delegate object? EnvironmentExecution(Element origin, CompilerState state, Argument[] objects, Argument[] arguments);

    public class Environment
    {
        public string Name { get; set; }
        public EnvironmentExecution Execution { get; set; }
        public ReadOnlyCollection<ParameterInfo> Parameters { get; set; }

        public Environment(string name, EnvironmentExecution execution, ParameterInfo[] parameters)
        {
            Name = name;
            Execution = execution;
            Parameters = new ReadOnlyCollection<ParameterInfo>(parameters);
        }
    }
}
