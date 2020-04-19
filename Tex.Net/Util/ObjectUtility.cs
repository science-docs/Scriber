using System;
using System.Collections.Generic;

namespace Tex.Net.Util
{
    public static class ObjectUtility
    {
        public static object[] ConvertToFlatArray(this object obj)
        {
            var list = new List<object>();
            ConvertFlatArrayInternal(obj, list);
            return list.ToArray();
        }

        private static void ConvertFlatArrayInternal(object obj, List<object> objects)
        {
            if (obj is Array arr)
            {
                foreach (var o in arr)
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
