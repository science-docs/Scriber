using Scriber.Language;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scriber.Engine
{
    public delegate object? CommandExecution(Element origin, CompilerState state, object?[] arguments);

    public class Command
    {
        public string Name { get; set; }
        public string? RequiredEnvironment { get; set; }
        public CommandExecution Execution { get; set; }
        public ParameterInfo[] Parameters { get; set; }

        public Command(string name, string? requiredEnvironment, CommandExecution execution, ParameterInfo[] parameters)
        {
            Name = name;
            RequiredEnvironment = requiredEnvironment;
            Execution = execution;
            Parameters = parameters;
        }
    }
}
