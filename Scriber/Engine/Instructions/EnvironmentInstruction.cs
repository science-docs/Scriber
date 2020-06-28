using Scriber.Language;
using Scriber.Util;
using System;

namespace Scriber.Engine.Instructions
{
    public class EnvironmentInstruction : EngineInstruction
    {
        public string Name { get; }

        public EnvironmentInstruction(Element origin) : base(origin)
        {
            Name = origin.Content ?? throw new InvalidOperationException();
        }

        public override object? Execute(CompilerState state, Argument[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidOperationException("An environment needs to contain at least one child block.");
            }

            var env = state.Environments.Find(Name);

            if (env == null)
            {
                throw new CompilerException(Origin, $"No environment found with name '{Name}'.");
            }

            var envArgs = args[0..^1];
            var envObjs = args[^1].Flatten();

            try
            {
                var obj = env.Execution(Origin, state, envObjs, envArgs);
                state.Context.Logger.Debug($"Successfully executed environment '{Name}'.");
                return obj;
            }
            catch (CompilerException e)
            {
                e.Origin ??= Origin;
                throw;
            }
            catch (Exception ex)
            {
                throw new CompilerException(Origin, $"Unhandled exception occured during execution of command '{Name}'.", ex);
            }
        }
    }
}
