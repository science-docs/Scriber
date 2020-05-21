using System;
using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class ObjectFieldInstruction : EngineInstruction
    {
        public string Key { get; }

        public ObjectFieldInstruction(Element element) : base(element)
        {
            Key = element.Content ?? throw new Exception();
        }

        public override object? Execute(CompilerState state, object?[] arguments)
        {
            return new ObjectField(Origin, Key, arguments[0]);
        }
    }
}
