using Scriber.Language;
using Scriber.Language.Syntax;
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
                        var command = new Command(name, CreateTemplateCommand(defaultValue), Array.Empty<Parameter>());

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

                    var command = new Command(name, CreateTemplateCommand(value), Array.Empty<Parameter>());

                    state.Commands.Add(command);
                }
            }
            
            var uri = state.Context.ResourceManager.RelativeUri(path);
            var resource = state.Context.ResourceManager.Get(uri);
            Compiler.Compile(state, resource);

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

            object? InvokeDynamic(SyntaxNode _, CompilerState __, Argument[] ___)
            {
                return value;
            }
        }
    }
}
