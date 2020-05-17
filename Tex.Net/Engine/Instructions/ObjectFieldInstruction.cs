using System;
using Tex.Net.Language;

namespace Tex.Net.Engine.Instructions
{
    public class ObjectFieldInstruction : EngineInstruction
    {
        public string Key { get; }

        public ObjectFieldInstruction(Element element)
        {
            Key = element.Content ?? throw new Exception();
        }

        public override object? Execute(CompilerState state, object[] arguments)
        {
            return new ObjectField(Key, arguments[0]);
        }
    }
}
