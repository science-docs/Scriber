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
        public static object?[] SortArguments(string command, CompilerState state, object?[] args, ParameterInfo[] parameters)
        {
            // Guard for when the command does not require any arguments
            if (parameters.Length == 0)
            {
                if (args.Length != 0)
                {
                    throw new CommandInvocationException($"Command {command} does not expect any arguments. Received: {args.Length}");
                }
                return Array.Empty<object>();
            }

            List<object?> objects = new List<object?>();

            CountParameters(parameters, out var hasState, out int required, out int optional);

            if (args.Length < required || args.Length > required + optional)
            {
                throw new CommandInvocationException($"Command {command} expects between {required} and {required + optional} arguments. {args.Length} arguments where supplied");
            }

            if (hasState)
            {
                objects.Add(state);
            }

            var suppliedOptional = args.Length - required;

            // switch positions of optional and required arguments with ranges
            objects.AddRange(args[suppliedOptional..^0]);
            objects.AddRange(args[0..suppliedOptional]);
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
        public static void MatchArguments(CompilerState state, object?[] args, ParameterInfo[] parameters)
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
                        throw new CommandInvocationException($"Argument 'null' was supplied for non nullable command parameter ({parm.ParameterType.Name} {parm.Name}).");
                    }
                    continue;
                }

                if (!IsAssignableFrom(state, parm, arg, out var transformed))
                {
                    throw new CommandInvocationException($"Object of type {arg.GetType().Name} cannot be assigned or transformed to parameter of type {parm.ParameterType.Name}");
                }
                else
                {
                    args[i] = transformed ?? throw new CommandInvocationException("Transformed element cannot be null");
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
                    throw new InvalidCastException($"Could not create object of type {nameof(ICommandArgument)} from type {target.Name}");
                }

                argument.Parse(str);
                transformed = argument;
                return true;
            }
            else if (target.IsArray && obj is ObjectArray list)
            {
                transformed = list.Get(target.GetElementType() ?? throw new Exception("Could not convert array type to simple type"));
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
