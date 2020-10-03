using Scriber.Language;
using Scriber.Util;
using System;
using System.Collections.Generic;

namespace Scriber.Engine
{
    public class ObjectArray : Traceable
    {
        private readonly Argument[] array;

        public CompilerState CompilerState { get; }

        public ObjectArray(Element origin, CompilerState compilerState, Argument[] objects) : base(origin)
        {
            CompilerState = compilerState ?? throw new ArgumentNullException(nameof(compilerState));
            array = objects ?? throw new ArgumentNullException(nameof(objects));
        }

        public Array Get(Type type)
        {
            return Get(type, null);
        }

        public Array Get(Type type, IEnumerable<Type>? overrides)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsArray)
            {
                throw new CompilerException(Origin, "Cannot create nested arrays.");
            }

            var arrType = type.MakeArrayType();
            var arr = Activator.CreateInstance(arrType, new object[] { array.Length }) as Array
                ?? throw new InvalidOperationException($"Could not create array of type '{type.FormattedName()}'.");

            for (int i = 0; i < arr.Length; i++)
            {
                var argument = array[i];

                if (argument.Value != null)
                {
                    if (DynamicDispatch.IsAssignableFrom(CompilerState, type, overrides, argument, out var transformed))
                    {
                        arr.SetValue(transformed, i);
                    }
                    else
                    {
                        throw new CompilerException(argument.Source, $"Cannot convert element of type '{argument.Value.GetType().FormattedName()}' to '{type.FormattedName()}' because no appropriate converter was found.");
                    }
                }
            }

            CompilerState.Context.Logger.Debug($"Created array of type {type.FormattedName()}.");

            return arr;
        }
    }
}
