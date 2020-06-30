using System;
using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class CommandInstruction : EngineInstruction
    {
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        public CommandInstruction(Element origin) : base(origin)
        {
            Name = origin.Content ?? throw new ArgumentNullException($"{nameof(origin)}.{nameof(origin.Content)}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CompilerException"/>
        public override object? Execute(CompilerState state, Argument[] arguments)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

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
