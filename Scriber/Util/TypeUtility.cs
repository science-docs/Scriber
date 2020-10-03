using Namotion.Reflection;
using Scriber.Engine;
using System;
using System.Text;

namespace Scriber.Util
{
    public static class TypeUtility
    {
        public static bool IsNullable(this ContextualType type)
        {
            var origType = type.OriginalType;
            if (Argument.IsArgumentType(origType))
            {
                type = type.GenericArguments[0];
            }
            return type.Nullability != Nullability.NotNullable;
        }

        public static string FormattedName(this Type type)
        {
            return FormattedName(type, null, false);
        }

        public static string FormattedName(this Type type, ContextualType? context, bool removeArgument = false)
        {
            if (type.IsGenericType && removeArgument)
            {
                var baseType = type.GetGenericTypeDefinition();
                if (baseType == typeof(Argument<>))
                {
                    return type.GenericTypeArguments[0].FormattedName(context?.GenericArguments[0], removeArgument);
                }
            }
            else if (removeArgument && type == typeof(Argument))
            {
                return typeof(object).FormattedName(context, removeArgument);
            }

            if (type.IsGenericType)
            {
                var sb = new StringBuilder();
                var name = type.Name;
                sb.Append(name.Substring(0, name.IndexOf('`')));
                sb.Append('<');
                var args = type.GenericTypeArguments;
                for (int i = 0; i < args.Length; i++)
                {
                    var contextArg = context?.GenericArguments[i];
                    var arg = args[i];
                    var argName = arg.FormattedName(contextArg, removeArgument);

                    sb.Append(argName);

                    if (i < args.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(">");
                if (context?.Nullability == Nullability.Nullable)
                {
                    sb.Append('?');
                }
                return sb.ToString();
            }
            else if (type.IsArray)
            {
                var eType = type.GetElementType()!;
                var arrayName = eType.FormattedName(context?.ElementType, removeArgument) + "[]";
                if (context?.Nullability == Nullability.Nullable)
                {
                    arrayName += "?";
                }
                return arrayName;
            }
            else
            {
                if (context?.Nullability == Nullability.Nullable)
                {
                    return type.Name + "?";
                }
                else
                {
                    return type.Name;
                }
            }
        }
    }
}
