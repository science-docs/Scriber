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

        public override object? Execute(CompilerState state, Argument[] arguments)
        {
            var command = state.Commands.Find(Name);

            if (command == null)
            {
                throw new CompilerException(Origin, $"Could not find command '{Name}'.");
            }

            try
            {
                var obj = command.Execution(Origin, state, arguments);
                state.Context.Logger.Debug($"Successfully executed command '{Name}'.");
                return obj;
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
