using Tex.Net.Layout.Document;

namespace Tex.Net.Engine.Commands
{
    [Package]
    public static class Layouts
    {
        [Command("centering")]
        public static CallbackArrangingBlock Centering(CompilerState state)
        {
            return new CallbackArrangingBlock(Center);

            static void Center(Block block)
            {
                block.Parent.Alignment = Layout.Alignment.Center;
            }
        }

        [Command("setlength")]
        public static void SetLength(CompilerState state, string lengthName, string length)
        {
            var l = double.Parse(length);
            state.Document.Variables[DocumentVariables.Length][lengthName].SetValue(l);
        }
    }
}
