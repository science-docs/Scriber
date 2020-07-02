using Scriber.Language;
using Scriber.Logging;
using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Scriber.Engine
{
    public static class DynamicDispatch
    {
        /// <summary>
        /// <para>Pads the given arguments so that they fit the specified parameter infos.</para>
        /// <para>Also transforms <c>params</c> variable length arrays into CLR Arrays.</para>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="state"></param>
        /// <param name="args"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="CompilerException"/>
        public static object[] PadArguments(string command, Element element, CompilerState state, Argument[] args, ParameterInfo[] parameters)
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

            var objects = new List<object>(parameters.Length);

            CountParameters(parameters, out var hasState, out int required, out int optional);

            if (args.Length < required || args.Length > required + optional && !IsParams(parameters[^1]))
            {
                throw new CompilerException(element, $"Command {command} expects between {required} and {required + optional} arguments. {args.Length} arguments where supplied");
            }

            if (hasState)
            {
                objects.Add(state);
            }

            if (IsParams(parameters[^1]) && args.Length >= required + optional)
            {
                objects.AddRange(TransformParamsArray(state, required + optional, args));
            }
            else
            {
                // add all given arguments
                objects.AddRange(args);
                // pad missing optional arguments
                var padArray = new object[required + optional - args.Length];
                objects.AddRange(padArray);
            }
            
            return objects.ToArray();
        }

        private static Argument[] TransformParamsArray(CompilerState state, int count, Argument[] source)
        {
            if (source.Length < count)
            {
                return source;
            }

            var args = new Argument[count];
            var index = count - 1;

            for (int i = 0; i < index; i++)
            {
                args[i] = source[i];
            }

            var restArgs = source[index..];

            var lastArg = new Argument(restArgs[0].Source, new ObjectArray(restArgs[0].Source, state, restArgs));
            args[^1] = lastArg;

            return args;
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

                        if (!transformed.GetType().IsAssignableFrom(argument.Value.GetType()))
                        {
                            state.Context.Logger.Debug(SR.Get(SRID.TransformSuccess, argument.Value.GetType().FormattedName(), transformed.GetType().FormattedName()));
                        }
                    }
                }
            }
        }

        public static bool IsAssignableFrom(CompilerState state, ParameterInfo parameter, Argument argument, out object? transformed)
        {
            transformed = null;
            var paramType = parameter.ParameterType;
            var isArgument = Argument.IsArgumentType(paramType, out var targetType);
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
            else if (state.Converters.TryConvert(value, targetType, out var convertedValue))
            {
                transformed = convertedValue;
            }

            if (isArgument && transformed != null)
            {
                transformed = MakeArgument(argument, targetType, transformed);
            }
            
            return transformed != null;
        }

        private static bool IsParams(ParameterInfo parameter)
        {
            return parameter.IsDefined(typeof(ParamArrayAttribute));
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
