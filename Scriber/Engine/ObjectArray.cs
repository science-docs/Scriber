using Scriber.Language;
using System;

namespace Scriber.Engine
{
    public class ObjectArray : Traceable
    {
        private readonly object?[] array;

        public ObjectArray(Element origin, object?[] objects) : base(origin)
        {
            array = objects;
        }

        public Array Get(Type type)
        {
            if (type.IsArray)
            {
                throw new CompilerException("Cannot ");
            }

            var arrType = type.MakeArrayType();
            var arr = Activator.CreateInstance(arrType, new object[] { array.Length }) as Array 
                ?? throw new Exception("Could not create array of type " + type.Name);

            for (int i = 0; i < arr.Length; i++)
            {
                var value = array[i];

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
                        var transformed = converter.Convert(value, type, new CompilerState());
                        arr.SetValue(transformed, i);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            return arr;
        }
    }
}
