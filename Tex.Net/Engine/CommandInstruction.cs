using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public class CommandInstruction : EngineInstruction
    {
        public string Name { get; set; }

        public CommandInstruction(string name)
        {
            Name = name;
        }

        public static new CommandInstruction Create(Element element)
        {
            return new CommandInstruction(element.Content);
        }

        public override object Execute(CompilerState state, object[] arguments)
        {
            var command = CommandCollection.Find(Name);
            return command.Execution(state, arguments);
        }
    }
}
