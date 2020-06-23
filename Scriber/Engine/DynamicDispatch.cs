using Scriber.Language;
using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Scriber.Engine
{
    public static class DynamicDispatch
    {
        /// <summary>
        /// <para>Sorts the given arguments so that they fit the specified parameter infos.</para>
        /// <para>Converts for example: <code>\x[a]{b}{c}</code> into the call: <code>x(b, c, a);</code></para>
        /// <para>This requires that the 'a' parameter is marked as optional via :<code>a = default</code></para>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="state"></param>
        /// <param name="args"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="CommandInvocationException"/>
        public static object[] SortArguments(string command, Element element, CompilerState state, Argument[] args, ParameterInfo[] parameters)
        {
            // Guard for when the command does not require any arguments
            if (parameters.Length == 0)
            {
                if (args.Length != 0)
                {
                    throw new CompilerException(element, $"Command {command} does not expect any arguments. Received: {args.Length}");
                }
                return Array.Empty<object>();
            }

            List<object> objects = new List<object>();

            CountParameters(parameters, out var hasState, out int required, out int optional);

            if (args.Length < required || args.Length > required + optional)
            {
                throw new CompilerException(element, $"Command {command} expects between {required} and {required + optional} arguments. {args.Length} arguments where supplied");
            }

            if (hasState)
            {
                objects.Add(state);
            }

            // add all given arguments
            objects.AddRange(args);
            // pad missing optional arguments
            var padArray = new object[required + optional - args.Length];
            objects.AddRange(padArray);

            return objects.ToArray();
        }

        /// <summary>
        /// <para>Matches the given arguments to the parameters of the command. If an argument is not directly assignable to the parameter type a conversion is attempted.</para>
        /// <para>A conversion requires an <see cref="IElementConverter"/> for the specified argument type to be present.</para>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="parameters"></param>
        public static void MatchArguments(CompilerState state, object[] args, ParameterInfo[] parameters)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var parm = parameters[i];

                if (arg is Argument argument)
                {
                    if (argument.Value == null)
                    {
                        var attribute = parm.GetCustomAttribute<ArgumentAttribute>();
                        if (attribute?.NonNull ?? false || parm.ParameterType.IsValueType)
                        {
                            throw new CompilerException(argument.Source, $"Argument 'null' was supplied for non nullable command parameter ({parm.ParameterType.FormattedName()} {attribute?.Name ?? parm.Name}).");
                        }
                        continue;
                    }

                    if (!IsAssignableFrom(state, parm, argument, out var transformed))
                    {
                        throw new CompilerException(argument.Source, $"Object of type '{argument.Value.GetType().FormattedName()}' cannot be assigned or transformed to parameter of type '{parm.ParameterType.FormattedName()}'.");
                    }
                    else
                    {
                        args[i] = transformed ?? throw new CompilerException(argument.Source, "Transformed element cannot be null.");

                        if (arg.GetType() != transformed.GetType())
                        {
                            state.Issues.Log(argument.Source, $"Transformed element of type '{argument.Value.GetType().FormattedName()}' to '{transformed.GetType().FormattedName()}'.");
                        }
                    }
                }
            }
        }

        public static bool IsAssignableFrom(CompilerState state, ParameterInfo parameter, Argument argument, out object? transformed)
        {
            transformed = null;
            var paramType = parameter.ParameterType;
            var isArgument = IsArgument(paramType, out var targetType);
            var value = argument.Value!;
            if (paramType == typeof(Argument))
            {
                transformed = argument;
            }
            else if (paramType == typeof(object))
            {
                transformed = argument.Value;
            }
            else if (paramType == typeof(object[]) && argument.Value!.GetType().IsArray)
            {
                var array = (Array)argument.Value!;
                var objArray = new object[array.Length];
                Array.Copy(array, 0, objArray, 0, array.Length);
                transformed = objArray;
            }
            else if (targetType.IsAssignableFrom(value.GetType()))
            {
                transformed = argument.Value;
            }
            else if (targetType.IsArray && value is ObjectArray list)
            {
                transformed = list.Get(targetType.GetElementType() ?? throw new Exception("Could not convert array type to simple type."));
            }
            else if (value is ObjectCreator creator)
            {
                transformed = creator.Create(parameter);
            }
            else // in the last instance look for a fitting converter
            {
                var converter = ElementConverters.Find(value.GetType(), targetType);
                if (converter != null)
                {
                    transformed = converter.Convert(value, targetType, state);
                }
            }

            if (isArgument && transformed != null)
            {
                transformed = MakeArgument(argument, targetType, transformed);
            }
            
            return transformed != null;
        }

        private static bool IsArgument(Type type, out Type genericType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Argument<>))
            {
                genericType = type.GenericTypeArguments[0];
                return true;
            }
            genericType = type;
            return false;
        }

        private static object MakeArgument(Argument argument, Type target, object value)
        {
            var targetArgType = typeof(Argument<>).MakeGenericType(target);
            var genericArg = Activator.CreateInstance(targetArgType, argument.Source, value)
                ?? throw new InvalidOperationException("A newly created argument cannot be null");
            return genericArg;
        }

        private static void CountParameters(ParameterInfo[] parameters, out bool hasState, out int required, out int optional)
        {
            int start = 0;
            required = 0;
            optional = 0;

            hasState = parameters[0].ParameterType == typeof(CompilerState);

            if (hasState)
            {
                start = 1;
            }

            for (int i = start; i < parameters.Length; i++)
            {
                if (parameters[i].IsOptional)
                {
                    optional++;
                }
                else
                {
                    required++;
                }
            }
        }
    }
}
