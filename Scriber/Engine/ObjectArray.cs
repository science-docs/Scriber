using Scriber.Language;
using Scriber.Util;
using System;

namespace Scriber.Engine
{
    public class ObjectArray : Traceable
    {
        private readonly Argument[] array;

        public CompilerState? CompilerState { get; }

        public ObjectArray(Element origin, CompilerState? compilerState, Argument[] objects) : base(origin)
        {
            CompilerState = compilerState;
            array = objects;
        }

        public Array Get(Type type)
        {
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
                var value = argument.Value;

                if (value is ObjectCreator creator)
                {
                    value = creator.Create(type, null);
                }

                if (value == null || type.IsAssignableFrom(value.GetType()))
                {
                    arr.SetValue(value, i);
                }
                else
                {
                    var converter = ElementConverters.Find(value.GetType(), type);
                    if (converter != null)
                    {
                        var transformed = converter.Convert(value, type, CompilerState ?? new CompilerState());
                        arr.SetValue(transformed, i);
                    }
                    else
                    {
                        throw new CompilerException(argument.Source, $"Cannot convert element of type '{value.GetType().FormattedName()}' to '{type.FormattedName()}' because no appropriate converter was found.");
                    }
                }
            }

            CompilerState?.Issues.Log(Origin, $"Created array of type {type.FormattedName()}.");

            return arr;
        }
    }
}
