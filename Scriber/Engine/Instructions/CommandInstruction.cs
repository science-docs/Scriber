using System;
using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class CommandInstruction : EngineInstruction
    {
        public string Name { get; set; }

        public CommandInstruction(Element origin) : base(origin)
        {
            Name = origin.Content ?? throw new InvalidOperationException("Command with null content was given");
        }

        public override object? Execute(CompilerState state, object?[] arguments)
        {
            var command = CommandCollection.Find(Name);

            if (command == null)
            {
                throw new CompilerException(Origin, "Could not find command: " + Name);
            }

            //if (command.RequiredEnvironment != null && state.Blocks.Current.Name != command.RequiredEnvironment)
            //{
            //    throw new CompilerException(Origin, $"Command {command.Name} requires environment {command.RequiredEnvironment}. Current environment is {state.Blocks.Current.Name}");
            //}

            try
            {
                return command.Execution(state, arguments);
            }
            catch (CompilerException e)
            {
                e.Origin ??= Origin;
                throw;
            }
            catch (Exception ex)
            {
                throw new CompilerException(Origin, "Unhandled exception occured during execution of command " + Name, ex);
            }
        }
    }
}
