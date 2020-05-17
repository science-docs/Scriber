using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tex.Net.Engine
{
    public class ObjectCreator
    {
        public string? Type { get; set; }
        public List<ObjectField> Fields { get; } = new List<ObjectField>();

        public object Create(ParameterInfo parameter)
        {
            var obj = CreateEmpty(parameter, out var type);

            var props = type.GetProperties();

            var matched = MatchFields(props, Fields);

            foreach (var match in matched)
            {
                FillField(obj, match.Item1, match.Item2.Value);
            }

            return obj;
        }

        private object CreateEmpty(ParameterInfo parameter, out Type type)
        {
            Type? objType = null;
            var attribute = parameter.GetCustomAttribute<ArgumentAttribute>();

            if (attribute?.Overrides != null && Type != null)
            {
                foreach (var t in attribute.Overrides)
                {
                    if (t.Name == Type || t.FullName == Type)
                    {
                        objType = t;
                        break;
                    }
                }
                if (objType == null)
                {
                    throw new Exception("Could not find specified type in argument override types");
                }
            }
            else
            {
                objType = parameter.ParameterType;
            }

            type = objType ?? throw new Exception();
            
            return Activator.CreateInstance(type) ?? throw new Exception();
        }

        private static List<Tuple<PropertyInfo, ObjectField>> MatchFields(IEnumerable<PropertyInfo> properties, IEnumerable<ObjectField> fields)
        {
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

            return list;
        }

        private static void FillField(object obj, PropertyInfo info, object value)
        {
            var type = info.PropertyType;

            if (!type.IsAssignableFrom(value.GetType()))
            {
                value = ElementConverters.Convert(value, type);
            }

            info.SetValue(obj, value);
        }
    }
}
