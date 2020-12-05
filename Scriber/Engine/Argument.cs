using Scriber.Language.Syntax;
using Scriber.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scriber.Engine
{
    public class Argument
    {
        public object? Value { get; }
        public SyntaxNode Source { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"/>
        public Argument(SyntaxNode source, object? value)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Value = value;
        }

        public Argument[] Flatten()
        {
            var list = new List<Argument>();
            Flatten(this, list);
            return list.ToArray();
        }

        public Argument MakeGeneric(Type type, object value)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (value != null)
            {
                var valueType = value.GetType();
                if (!type.IsAssignableFrom(valueType))
                {
                    throw new InvalidCastException($"Cannot create generic argument of type {type.FormattedName()} with value of type {valueType.FormattedName()}.");
                }
            }

            var targetArgType = MakeGeneric(type);
            var genericArg = Activator.CreateInstance(targetArgType, Source, value) as Argument
                ?? throw new InvalidOperationException("A newly created argument cannot be null");
            return genericArg;
        }

        public static Type MakeGeneric(Type type)
        {
            return typeof(Argument<>).MakeGenericType(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public static bool IsArgumentType(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Argument<>);
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public static bool IsArgumentType(Type type, out Type genericType)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (IsArgumentType(type))
            {
                genericType = type.GenericTypeArguments[0];
                return true;
            }
            else
            {
                genericType = type;
                return false;
            }
        }

        private static void Flatten(Argument argument, List<Argument> list)
        {
            if (argument.Value is Argument argValue)
            {
                Flatten(argValue, list);
            }
            if (argument.Value is IEnumerable<Argument> arguments)
            {
                foreach (var arg in arguments)
                {
                    Flatten(arg, list);
                }
            }
            else if (argument.Value is string)
            {
                list.Add(argument);
            }
            else if (argument.Value is IEnumerable elements)
            {
                foreach (var element in elements)
                {
                    Flatten(new Argument(argument.Source, element), list);
                }
            }
            else
            {
                list.Add(argument);
            }
        }
    }

    public class Argument<T> : Argument
    {
        public new T Value { get; }

        public Argument(SyntaxNode source, T value) : base(source, value)
        {
            Value = value;
        }
    }
}
