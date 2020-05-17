using System;
using Tex.Net.Language;

namespace Tex.Net.Engine.Instructions
{
    public class CommandInstruction : EngineInstruction
    {
        public string Name { get; set; }

        public CommandInstruction(string name)
        {
            Name = name;
        }

        public static new CommandInstruction Create(Element element)
        {
            return new CommandInstruction(element.Content ?? throw new InvalidOperationException("Command with null content was given"));
        }

        public override object? Execute(CompilerState state, object[] arguments)
        {
            var command = CommandCollection.Find(Name);

            if (command == null)
            {
                throw new CommandInvocationException("Could not find command: " + Name);
            }

            if (command.RequiredEnvironment != null && state.Blocks.Current.Name != command.RequiredEnvironment)
            {
                throw new CommandInvocationException($"Command {command.Name} requires environment {command.RequiredEnvironment}. Current environment is {state.Blocks.Current.Name}");
            }

            try
            {
                return command.Execution(state, arguments);
            }
            catch (CommandInvocationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CommandInvocationException("Unhandled exception occured during execution of command " + Name, ex);
            }
        }
    }
}
