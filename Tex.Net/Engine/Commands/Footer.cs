using Tex.Net.Layout.Document;

namespace Tex.Net.Engine.Commands
{
    [Package]
    public static class Footer
    {
        [Command("cfooter")]
        public static void CenterFooter(CompilerState state, Paragraph content)
        {
            SetFixedBlock(state, content, FixedPosition.BottomCenter);
        }

        private static void SetFixedBlock(CompilerState state, Paragraph content, FixedPosition position)
        {
            var block = new FixedBlock(content)
            {
                Position = FixedPosition.BottomCenter
            };
            state.Document.PageItems.Add(block);
        }
    }
}
