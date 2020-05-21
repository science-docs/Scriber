﻿using Scriber.Language;
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

        public override object? Execute(CompilerState state, object?[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidOperationException("An environment needs to contain at least one child block.");
            }

            var env = EnvironmentCollection.Find(Name);

            if (env == null)
            {
                throw new CompilerException(Origin, $"No environment found with name '{Name}'.");
            }

            var envArgs = args[0..^1];
            var envObjs = args[^1].ConvertToFlatArray();

            return env.Execution(state, envObjs, envArgs);
        }
    }
}
