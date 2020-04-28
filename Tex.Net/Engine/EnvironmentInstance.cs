using System.Reflection;

namespace Tex.Net.Engine
{
    public delegate object? EnvironmentExecution(CompilerState state, object[] objects, object[] arguments);

    public class Environment
    {
        public string Name { get; set; }
        public EnvironmentExecution Execution { get; set; }
        public ParameterInfo[] Parameters { get; set; }

        public Environment(string name, EnvironmentExecution execution, ParameterInfo[] parameters)
        {
            Name = name;
            Execution = execution;
            Parameters = parameters;
        }
    }
}
