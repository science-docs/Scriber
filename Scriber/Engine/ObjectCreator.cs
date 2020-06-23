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
        public CompilerState? CompilerState { get; set; }
        public List<ObjectField> Fields { get; } = new List<ObjectField>();

        public ObjectCreator(Element origin) : this(origin, null)
        {
        }

        public ObjectCreator(Element origin, CompilerState? compilerState) : base(origin)
        {
            CompilerState = compilerState;
        }

        public object Create(ParameterInfo parameter)
        {
            var attribute = parameter.GetCustomAttribute<ArgumentAttribute>();
            return Create(parameter.ParameterType, attribute?.Overrides);
        }

        public object Create(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<ObjectFieldAttribute>();
            return Create(property.PropertyType, attribute?.Overrides);
        }

        public object Create(Type defaultType, Type[]? overrides)
        {
            var obj = CreateEmpty(defaultType, overrides, out var type);

            if (obj is DocumentVariable vars)
            {
                foreach (var field in Fields)
                {
                    FillField(vars, field);
                }
            }
            else
            {
                var matched = MatchFields(type, Fields);
                foreach (var match in matched)
                {
                    FillField(obj, match.Item1, match.Item2.Argument.Value);
                }
            }

            CompilerState?.Issues.Log(Origin, $"Created object of type '{obj.GetType().FormattedName()}'.");

            return obj;
        }

        private object CreateEmpty(Type defaultType, Type[]? overrides, out Type type)
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

            type = objType;

            ValidateType(type);

            object? emptyObj;

            try
            {
                emptyObj = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new CompilerException(TypeElement ?? Origin, $"An exception occured while creating object of type '{type.FormattedName()}'.", ex);
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

            if (type.IsAbstract)
            {
                issue = "is defined as abstract";
            }
            else if (type.IsInterface)
            {
                issue = "is an interface";
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
            else if (!type.IsClass && !type.IsValueType)
            {
                issue = "is not a data type";
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

        private List<Tuple<PropertyInfo, ObjectField>> MatchFields(Type type, IEnumerable<ObjectField> fields)
        {
            var properties = type.GetProperties();
            var list = new List<Tuple<PropertyInfo, ObjectField>>();
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
                        list.Add(new Tuple<PropertyInfo, ObjectField>(prop, field));
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

        private static void FillField(object obj, PropertyInfo info, object? value)
        {
            var type = info.PropertyType;

            if (value is ObjectCreator subCreator)
            {
                value = subCreator.Create(info);
            }

            if (value != null && !type.IsAssignableFrom(value.GetType()))
            {
                value = ElementConverters.Convert(value, type);
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
