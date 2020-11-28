using Scriber.Language.Syntax;
using Scriber.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Scriber.Engine
{
    public class ObjectCreator : Traceable<ObjectSyntax>
    {
        public string? TypeName { get; set; }
        //public Element? TypeElement { get; set; }
        public CompilerState CompilerState { get; set; }
        public List<ObjectField> Fields { get; } = new List<ObjectField>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="compilerState"></param>
        /// <exception cref="ArgumentNullException"/>
        public ObjectCreator(ObjectSyntax origin, CompilerState compilerState) : base(origin)
        {
            CompilerState = compilerState ?? throw new ArgumentNullException(nameof(compilerState));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CompilerException"/>
        public object Create(ParameterInfo parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var attribute = parameter.GetCustomAttribute<ArgumentAttribute>();
            return Create(parameter.ParameterType, attribute?.Overrides);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CompilerException"/>
        public object Create(PropertyInfo property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.GetCustomAttribute<ObjectFieldAttribute>();
            return Create(property.PropertyType, attribute?.Overrides);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultType"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CompilerException"/>
        public object Create(Type defaultType, IEnumerable<Type>? overrides)
        {
            if (defaultType is null)
            {
                throw new ArgumentNullException(nameof(defaultType));
            }

            var obj = CreateEmpty(defaultType, overrides);

            if (obj is DynamicDictionary vars)
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

        private object CreateEmpty(Type defaultType, IEnumerable<Type>? overrides)
        {
            Type? objType = null;

            if (TypeName != null && TypeName != defaultType.Name && TypeName != defaultType.FullName)
            {
                if (overrides != null)
                {
                    foreach (var t in overrides)
                    {
                        if (t.Name == TypeName || t.FullName == TypeName)
                        {
                            objType = t;
                            break;
                        }
                    }
                }
                if (objType == null)
                {
                    throw new CompilerException(Origin, $"Object of type '{TypeName}' cannot be created. It is not a member of the overrides for type '{defaultType.FormattedName()}'.");
                }
            }
            else
            {
                objType = defaultType;
            }

            if (objType == typeof(object) || typeof(DynamicObject).IsAssignableFrom(objType))
            {
                objType = typeof(DynamicDictionary);
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
                throw new CompilerException(Origin, $"An exception occured while creating object of type '{objType.FormattedName()}'.", ex);
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

                if (emptyObj != null)
                {
                    foreach (var property in type.GetProperties().Where(e => e.CanWrite))
                    {
                        var defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();

                        if (defaultValueAttribute != null)
                        {
                            property.SetValue(emptyObj, defaultValueAttribute.Value);
                        }
                    }
                }
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
                throw new CompilerException(Origin, $"Type '{type.FormattedName()}' {issue}. An object of this type cannot be instantiated.");
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

            if (value is ObjectArray objectArray)
            {
                var elementType = type.GetElementType();
                if (type.IsArray && elementType != null)
                {
                    value = objectArray.Get(elementType, null);
                }
                else
                {
                    throw new CompilerException();
                }
            }

            if (value != null && !type.IsAssignableFrom(value.GetType()))
            {
                CompilerState.Converters.TryConvert(value, type, out value);
            }

            info.SetValue(obj, value);
        }

        private static void FillField(DynamicDictionary vars, ObjectField field)
        {
            var value = field.Argument.Value;

            if (value is Argument[] array && array.Length == 1)
            {
                value = array[0].Value;
            }

            if (value is ObjectCreator subCreator)
            {
                // We can assume that the returned value is a document variable.
                // In every other case the Create call throws an exception.
                value = (subCreator.Create(typeof(DynamicDictionary), null) as DynamicObject)!;
            }
            else if (value is ObjectArray objectArray)
            {
                value = objectArray.Get(typeof(object), null);
            }

            vars.SetMember(field.Key, value);
        }
    }
}
