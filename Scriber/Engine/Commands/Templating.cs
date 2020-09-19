using Scriber.Language;
using Scriber.Util;
using System;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Templating
    {
        [Command("DefineTemplate")]
        public static void DefineTemplate(CompilerState state, DynamicDictionary? defaults = null, DynamicDictionary? documentation = null)
        {
            if (defaults != null)
            {
                var objects = defaults.GetContents();

                foreach (var obj in objects)
                {
                    var name = obj.Key;
                    var defaultValue = obj.Value;

                    var existingCommand = state.Commands.Find(name);

                    if (existingCommand == null)
                    {
                        var command = new Command(name, CreateTemplateCommand(defaultValue), Array.Empty<System.Reflection.ParameterInfo>());

                        state.Commands.Add(command);
                    }
                }
            }
        }

        [Command("UseTemplate")]
        public static void UseTemplate(CompilerState state, string path, DynamicDictionary? dynamicDictionary = null)
        {
            if (dynamicDictionary != null)
            {
                var objects = dynamicDictionary.GetContents();

                // Add arguments of template as global commands
                foreach (var obj in objects)
                {
                    var name = obj.Key;
                    var value = obj.Value;

                    var command = new Command(name, CreateTemplateCommand(value), Array.Empty<System.Reflection.ParameterInfo>());

                    state.Commands.Add(command);
                }
            }
            

            var uri = state.FileSystem.Path.ConvertToUri(path);
            var resource = state.Context.ResourceSet.Get(uri);
            var text = resource.GetContentAsString();

            var tokens = Lexer.Tokenize(text);
            var elements = Parser.Parse(tokens, resource, state.Context.Logger);
            Compiler.Compile(state, elements.Elements);

            if (dynamicDictionary != null)
            {
                var objects = dynamicDictionary.GetContents();
                // Remove arguments again 
                foreach (var obj in objects)
                {
                    state.Commands.Remove(obj.Key);
                }
            }
        }

        private static CommandExecution CreateTemplateCommand(object? value)
        {
            return InvokeDynamic;

            object? InvokeDynamic(Element _, CompilerState __, Argument[] ___)
            {
                return value;
            }
        }
    }
}
