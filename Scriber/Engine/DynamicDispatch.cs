using Scriber.Language;
using Scriber.Logging;
using Scriber.Util;
using System;
using System.Collections.Generic;

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
        public static object[] PadArguments(string command, Element element, CompilerState state, Argument[] args, Parameter[] parameters)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (state is null)
                throw new ArgumentNullException(nameof(state));
            if (args is null)
                throw new ArgumentNullException(nameof(args));
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            var objects = new List<object>(parameters.Length);

            CountParameters(parameters, out var hasState, out int required, out int optional);
            var full = required + optional;

            if (args.Length < required || (args.Length > full && parameters.Length > 0 && !IsArrayParameter(parameters[^1], full, args)))
            {
                throw new CompilerException(element, $"Command {command} expects between {required} and {required + optional} arguments. {args.Length} arguments where supplied");
            }

            if (hasState)
            {
                objects.Add(state);
            }

            if (parameters.Length > 0 && IsArrayParameter(parameters[^1], full, args) && args.Length >= full)
            {
                objects.AddRange(TransformParamsArray(state, full, args));
            }
            else if (full > 0)
            {
                // add all given arguments
                objects.AddRange(args);
                // pad missing optional arguments with null
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
        /// <para>A conversion requires an <see cref="IConverter"/> for the specified argument type to be present.</para>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="parameters"></param>
        public static object[] MatchArguments(CompilerState state, object[] args, Parameter[] parameters)
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state));
            if (args is null)
                throw new ArgumentNullException(nameof(args));
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            var result = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var parm = parameters[i];

                if (arg is Argument argument)
                {
                    if (argument.Value == null)
                    {
                        if (!parm.IsNullable || parm.Type.IsValueType)
                        {
                            throw new CompilerException(argument.Source, $"Argument 'null' was supplied for non nullable command parameter '{parm.ToString(true)}'.");
                        }
                        // result[i] is null by default, therefore continue
                        continue;
                    }

                    if (!IsAssignableFrom(state, parm.Type, parm.Overrides, argument, out var transformed))
                    {
                        throw new CompilerException(argument.Source, $"Object of type '{argument.Value.GetType().FormattedName()}' cannot be assigned or transformed to parameter of type '{parm.Type.FormattedName(parm.Info, true)}'.");
                    }
                    else
                    {
                        result[i] = transformed ?? throw new CompilerException(argument.Source, "Transformed element cannot be null.");

                        if (!transformed.GetType().IsAssignableFrom(argument.Value.GetType()))
                        {
                            state.Context.Logger.Debug(SR.Get(SRID.TransformSuccess, argument.Value.GetType().FormattedName(), transformed.GetType().FormattedName(parm.Info)));
                        }
                    }
                }
                else
                {
                    result[i] = arg;
                }
            }
            return result;
        }

        public static bool IsAssignableFrom(CompilerState state, Type type, IEnumerable<Type>? overrides, Argument argument, out object? transformed)
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (argument is null)
                throw new ArgumentNullException(nameof(argument));

            if (argument.Value == null)
            {
                transformed = null;
                return false;
            }

            var isArgument = Argument.IsArgumentType(type, out var argType);
            transformed = TryConvertSingle(state, type, overrides, argument);
            
            if (transformed == null)
            {
                transformed = TryConvertArray(state, type, overrides, argument);
            }

            if (isArgument && transformed != null)
            {
                transformed = argument.MakeGeneric(argType, transformed);
            }

            return transformed != null;
        }

        private static object? TryConvertSingle(CompilerState state, Type type, IEnumerable<Type>? overrides, Argument argument)
        {
            var value = argument.Value!;
            Argument.IsArgumentType(type, out var argType);

            if (type == typeof(Argument))
            {
                return argument;
            }
            else if (value is ObjectCreator creator)
            {
                return creator.Create(argType, overrides);
            }
            else if (argType == typeof(object) || argType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }
            else if (state.Converters.TryConvert(value, argType, out var convertedValue))
            {
                return convertedValue;
            }

            return null;
        }

        private static object? TryConvertArray(CompilerState state, Type type, IEnumerable<Type>? overrides, Argument argument)
        {
            Array? array = null;
            var value = argument.Value!;
            Argument.IsArgumentType(type, out var argType);
            var innerArgument = false;

            if (type.IsArray)
            {
                var baseType = type.GetElementType()!;
                innerArgument = Argument.IsArgumentType(baseType, out argType);
            }

            if (argType.IsArray)
            {
                argType = argType.GetElementType()!;
            }

            if (value is ObjectArray objectArray)
            {
                var arrayType = argType;
                if (innerArgument)
                {
                    arrayType = Argument.MakeGeneric(arrayType);
                }
                array = objectArray.Get(arrayType, overrides);
            }
            else if (value.GetType().IsArray)
            {
                array = (Array)value;
            }

            if (array != null)
            {
                return DispatchArray(state, array, argType, innerArgument, argument);
            }

            return null;
        }

        private static Array DispatchArray(CompilerState state, Array array, Type argType, bool innerArgument, Argument argument)
        {
            var arrayType = argType;
            if (innerArgument)
            {
                arrayType = Argument.MakeGeneric(argType);
            }
            var newArray = Array.CreateInstance(arrayType, array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                var item = array.GetValue(i);
                var itemValue = item;

                if (itemValue is Argument argumentItem)
                {
                    itemValue = argumentItem.Value;
                }

                if (itemValue != null)
                {
                    if (argType.IsAssignableFrom(itemValue.GetType()))
                    {
                        // do nothing
                    }
                    else if (state.Converters.TryConvert(itemValue, argType, out var convertedValue))
                    {
                        itemValue = convertedValue;
                    }

                    if (innerArgument && item is Argument argItem)
                    {
                        itemValue = argItem.MakeGeneric(argType, itemValue);
                    }
                    else if (innerArgument)
                    {
                        itemValue = argument.MakeGeneric(argType, itemValue);
                    }
                    else if (arrayType == typeof(Argument))
                    {
                        itemValue = new Argument(argument.Source, itemValue);
                    }
                }

                newArray.SetValue(itemValue, i);
            }

            return newArray;
        }

        private static bool IsArrayParameter(Parameter parameter, int size, Argument[] args)
        {
            if (parameter.Type.IsArray)
            {
                if (args.Length == size)
                {
                    var value = args[^1].Value;
                    if (value != null)
                    {
                        return !value.GetType().IsArray;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private static void CountParameters(Parameter[] parameters, out bool hasState, out int required, out int optional)
        {
            int start = 0;
            required = 0;
            optional = 0;

            hasState = parameters.Length > 0 && parameters[0].Type == typeof(CompilerState);

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
