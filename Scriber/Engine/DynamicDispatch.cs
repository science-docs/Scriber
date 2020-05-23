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
        public static object?[] SortArguments(string command, Element element, CompilerState state, object?[] args, ParameterInfo[] parameters)
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

            List<object?> objects = new List<object?>();

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
        public static void MatchArguments(CompilerState state, Element element, object?[] args, ParameterInfo[] parameters)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var parm = parameters[i];

                if (arg == null)
                {
                    var attribute = parm.GetCustomAttribute<ArgumentAttribute>();
                    if (attribute?.NonNull ?? false || parm.ParameterType.IsValueType)
                    {
                        throw new CompilerException(element, $"Argument 'null' was supplied for non nullable command parameter ({parm.ParameterType.FormattedName()} {attribute?.Name ?? parm.Name}).");
                    }
                    continue;
                }

                if (!IsAssignableFrom(state, parm, arg, out var transformed))
                {
                    throw new CompilerException(element, $"Object of type '{arg.GetType().FormattedName()}' cannot be assigned or transformed to parameter of type '{parm.ParameterType.FormattedName()}'.");
                }
                else
                {
                    args[i] = transformed ?? throw new CompilerException(element, "Transformed element cannot be null.");

                    if (arg.GetType() != transformed.GetType())
                    {
                        state.Issues.Log(element, $"Transformed element of type '{arg.GetType().FormattedName()}' to '{transformed.GetType().FormattedName()}'.");
                    }
                }
            }
        }

        public static bool IsAssignableFrom(CompilerState state, ParameterInfo parameter, object obj, out object? transformed)
        {
            var target = parameter.ParameterType;
            if (target.IsAssignableFrom(obj.GetType()))
            {
                transformed = obj;
                return true;
            }
            else if (typeof(ICommandArgument).IsAssignableFrom(target))
            {
                var stringConverter = ElementConverters.Find(obj.GetType(), typeof(string));
                if (stringConverter == null)
                {
                    throw new CommandInvocationException("");
                }

                if (!(stringConverter.Convert(obj, typeof(string), state) is string str))
                {
                    throw new Exception();
                }

                if (!(Activator.CreateInstance(target) is ICommandArgument argument))
                {
                    throw new InvalidCastException($"Could not create object of type {nameof(ICommandArgument)} from type {target.FormattedName()}.");
                }

                argument.Parse(str);
                transformed = argument;
                return true;
            }
            else if (target.IsArray && obj is ObjectArray list)
            {
                transformed = list.Get(target.GetElementType() ?? throw new Exception("Could not convert array type to simple type."));
                return true;
            }
            else if (obj is ObjectCreator creator)
            {
                transformed = creator.Create(parameter);
                return true;
            }

            var converter = ElementConverters.Find(obj.GetType(), target);
            if (converter != null)
            {
                transformed = converter.Convert(obj, target, state);
                return true;
            }

            transformed = null;
            return false;
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
