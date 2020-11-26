using System;
using System.Collections.Generic;
using Scriber.Language.Syntax;

namespace Scriber.Engine.Instructions
{
    public class CommandInstruction : EngineInstruction<CommandSyntax>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CompilerException"/>
        public override object? Execute(CompilerState state, CommandSyntax commandSyntax)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (commandSyntax is null)
            {
                throw new ArgumentNullException(nameof(commandSyntax));
            }

            if (commandSyntax.Name == null)
            {
                throw new CompilerException(commandSyntax, "");
            }

            var command = state.Commands.Find(commandSyntax.Name.Value, commandSyntax.Arguments!.Children);

            if (command == null)
            {
                throw new CompilerException(commandSyntax, $"Could not find command '{commandSyntax.Name.Value}'.");
            }

            var arguments = new List<Argument>();

            foreach (var argSyntax in commandSyntax.Arguments!.Children)
            {
                var argResult = EngineInstruction.Execute(state, argSyntax);
                arguments.Add(argResult);
            }

            try
            {
                var obj = command.Execution(commandSyntax, state, arguments.ToArray());
                state.Context.Logger.Debug($"Successfully executed command '{command.Name}'.");
                return obj;
            }
            catch (CompilerException e)
            {
                e.Origin ??= commandSyntax;
                throw;
            }
            catch (Exception ex)
            {
                throw new CompilerException(commandSyntax, "Unhandled exception occured during execution of command " + command.Name, ex);
            }
        }
    }
}
