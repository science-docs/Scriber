using Scriber.Language;
using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scriber.Engine
{
    public class ObjectCreator : Traceable
    {
        public string? TypeName { get; set; }
        public Element? TypeElement { get; set; }
        public CompilerState CompilerState { get; set; }
        public List<ObjectField> Fields { get; } = new List<ObjectField>();

        public ObjectCreator(Element origin, CompilerState compilerState) : base(origin)
        {
            CompilerState = compilerState ?? throw new ArgumentNullException(nameof(compilerState));
        }

        public object Create(ParameterInfo parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var attribute = parameter.GetCustomAttribute<ArgumentAttribute>();
            return Create(parameter.ParameterType, attribute?.Overrides);
        }

        public object Create(PropertyInfo property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.GetCustomAttribute<ObjectFieldAttribute>();
            return Create(property.PropertyType, attribute?.Overrides);
        }

        public object Create(Type defaultType, Type[]? overrides)
        {
            var obj = CreateEmpty(defaultType, overrides);

            if (obj is DocumentVariable vars)
            {
                foreach (var field in Fields)
                {
                    FillField(vars, field);
                }
            }
            else
            {
                var objToFill = AsArgumentValue(obj);
                var matched = MatchFields(objToFill.GetType(), Fields);
                foreach (var (property, field) in matched)
                {
                    FillField(objToFill, property, field.Argument.Value);
                }
            }

            CompilerState.Context.Logger.Debug($"Created object of type '{obj.GetType().FormattedName()}'.");

            return obj;
        }

        private object AsArgumentValue(object value)
        {
            if (Argument.IsArgumentType(value.GetType()) && value is Argument argument && argument.Value != null)
            {
                value = argument.Value;
            }
            return value;
        }

        private object CreateEmpty(Type defaultType, Type[]? overrides)
        {
            Type? objType = null;

            if (TypeName != null && TypeName != defaultType.Name && TypeName != defaultType.FullName)
            {
                foreach (var t in overrides ?? Array.Empty<Type>())
                {
                    if (t.Name == TypeName || t.FullName == TypeName)
                    {
                        objType = t;
                        break;
                    }
                }
                if (objType == null)
                {
                    throw new CompilerException(TypeElement ?? Origin, $"Object of type '{TypeName}' cannot be created. It is not a member of the overrides for type '{defaultType.FormattedName()}'.");
                }
            }
            else
            {
                objType = defaultType;
            }

            if (Argument.IsArgumentType(objType, out var genericType))
            {
                ValidateType(genericType);
            }
            else
            {
                ValidateType(objType);
            }

            try
            {
                return CreateInstance(objType);
            }
            catch (Exception ex)
            {
                throw new CompilerException(TypeElement ?? Origin, $"An exception occured while creating object of type '{objType.FormattedName()}'.", ex);
            }
        }

        private object CreateInstance(Type type)
        {
            object? emptyObj;
            if (Argument.IsArgumentType(type, out var genericType))
            {
                ValidateType(genericType);
                emptyObj = Activator.CreateInstance(type, Origin, CreateInstance(genericType));
            }
            else
            {
                emptyObj = Activator.CreateInstance(type);
            }

            // An object returned by Activator.CreateInstance(Type) can be null if the type is e.g. int?. 
            // Altough these types are not valid by the previous validation we still check for null here.
            if (emptyObj == null)
            {
                throw new InvalidOperationException("Null values created by an ObjectCreator are forbidden.");
            }

            return emptyObj;
        }

        private void ValidateType(Type type)
        {
            string? issue = null;

            if (type.IsInterface)
            {
                issue = "is an interface";
            }
            else if (type.IsAbstract)
            {
                issue = "is defined as abstract";
            }
            else if (type.IsPrimitive)
            {
                issue = "is a primitive";
            }
            else if (type.IsArray)
            {
                issue = "is an array type";
            }
            else if (type.IsEnum)
            {
                issue = "is an enum type";
            }
            else if (typeof(Delegate).IsAssignableFrom(type))
            {
                issue = "is a delegate type";
            }
            else if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                issue = "contains no default constructor";
            }

            if (issue != null)
            {
                throw new CompilerException(TypeElement ?? Origin, $"Type '{type.FormattedName()}' {issue}. An object of this type cannot be instantiated.");
            }
        }

        private List<(PropertyInfo property, ObjectField field)> MatchFields(Type type, IEnumerable<ObjectField> fields)
        {
            var properties = type.GetProperties();
            var list = new List<(PropertyInfo, ObjectField)>();
            var matched = new List<ObjectField>();

            foreach (var prop in properties)
            {
                var fieldAttribute = prop.GetCustomAttribute<ObjectFieldAttribute>();
                string name = fieldAttribute?.Name ?? prop.Name;

                foreach (var field in fields)
                {
                    if (field.Key == name)
                    {
                        matched.Add(field);
                        list.Add((prop, field));
                        break;
                    }
                }
            }

            if (CompilerState != null)
            {
                foreach (var unmatched in fields.Except(matched))
                {
                    CompilerState.Issues.Add(unmatched.Origin, CompilerIssueType.Warning, $"Property '{unmatched.Key}' not found on type '{type.Name}'.");
                }
            }
            

            return list;
        }

        private void FillField(object obj, PropertyInfo info, object? value)
        {
            var type = info.PropertyType;

            if (value is ObjectCreator subCreator)
            {
                value = subCreator.Create(info);
            }

            if (value != null && !type.IsAssignableFrom(value.GetType()))
            {
                CompilerState.Converters.TryConvert(value, type, out value);
            }

            info.SetValue(obj, value);
        }

        private static void FillField(DocumentVariable vars, ObjectField field)
        {
            var value = field.Argument.Value;
            DocumentVariable inner;

            if (value is ObjectCreator subCreator)
            {
                // We can assume that the returned value is a document variable.
                // In every other case the Create call throws an exception.
                inner = (subCreator.Create(typeof(DocumentVariable), null) as DocumentVariable)!;
            }
            else
            {
                inner = new DocumentVariable(value);
            }

            vars[field.Key] = inner;
        }
    }
}
