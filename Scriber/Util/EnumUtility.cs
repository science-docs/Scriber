using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Util
{
    public static class EnumUtility
    {
        public static string? GetName<TEnum>(TEnum value) where TEnum : struct
        {
            return EnumHelpers<TEnum>.ConvertBack(value);
        }

        public static bool TryParseEnum<TEnum>(string value, out TEnum output) where TEnum : struct
        {
            var parsed = ParseEnum<TEnum>(value);
            if (parsed == null)
            {
                output = default;
                return false;
            }
            else
            {
                output = parsed.Value;
                return true;
            }
        }

        public static TEnum? ParseEnum<TEnum>(string value) where TEnum : struct
        {
            return EnumHelpers<TEnum>.Convert(value);
        }

        private static class EnumHelpers<TTarget> where TTarget : struct
        {
            private static readonly Dictionary<string, TTarget> Dict = new Dictionary<string, TTarget>();
            private static readonly Dictionary<TTarget, string> DictBack = new Dictionary<TTarget, string>();

            static EnumHelpers()
            {
                var values = Enum.GetValues(typeof(TTarget));

                foreach (TTarget value in values.Cast<TTarget>())
                {
                    var stringValue = value.ToString()!.ToLowerInvariant();
                    Dict[stringValue] = value;
                    DictBack[value] = stringValue;
                }
            }

            public static string? ConvertBack(TTarget value)
            {
                if (DictBack.TryGetValue(value, out var target))
                {
                    return target;
                }
                else
                {
                    return null;
                }
            }

            public static TTarget? Convert(string value)
            {
                if (Dict.TryGetValue(value, out var target))
                {
                    return target;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
