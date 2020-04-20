using System.Linq;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine
{
    public class BlockInstruction : EngineInstruction
    {
        public override object Execute(CompilerState state, object[] arguments)
        {
            state.Environments.Current.Objects.Clear();
            if (arguments.All(e => e is Leaf))
            {
                var paragraph = new Paragraph();
                paragraph.Leaves.AddRange(arguments.Cast<Leaf>());
                return paragraph;
            }
            return arguments;
        }
    }
}