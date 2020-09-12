using System;
using System.Linq;
using System.Text;

namespace Scriber.Util
{
    public static class TypeUtility
    {
        public static string FormattedName(this Type type)
        {
            if (type.IsGenericType)
            {
                var sb = new StringBuilder();
                var name = type.Name;
                sb.Append(name.Substring(0, name.IndexOf('`')));
                sb.Append('<');
                sb.Append(string.Join(", ", type.GenericTypeArguments.Select(e => e.FormattedName())));
                sb.Append(">");
                return sb.ToString();
            }
            else
            {
                return type.Name;
            }
        }
    }
}
