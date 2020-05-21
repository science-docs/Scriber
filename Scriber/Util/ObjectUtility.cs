using System;
using System.Collections;
using System.Collections.Generic;

namespace Scriber.Util
{
    public static class ObjectUtility
    {
        public static object?[] ConvertToFlatArray(this object? obj)
        {
            var list = new List<object?>();
            ConvertFlatArrayInternal(obj, list);
            return list.ToArray();
        }

        private static void ConvertFlatArrayInternal(object? obj, List<object?> objects)
        {
            if (obj is IEnumerable enumerable)
            {
                foreach (var o in enumerable)
                {
                    ConvertFlatArrayInternal(o, objects);
                }
            }
            else
            {
                objects.Add(obj);
            }
        }
    }
}
