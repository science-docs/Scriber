using Scriber.Language;
using Scriber.Util;
using System;

namespace Scriber.Engine.Instructions
{
    public class EnvironmentInstruction : EngineInstruction
    {
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        public EnvironmentInstruction(Element origin) : base(origin)
        {
            Name = origin.Content ?? throw new ArgumentNullException($"{nameof(origin)}.{nameof(origin.Content)}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CompilerException"/>
        public override object? Execute(CompilerState state, Argument[] args)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length == 0)
            {
                throw new ArgumentException("An environment needs to contain at least one child block.", nameof(args));
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
