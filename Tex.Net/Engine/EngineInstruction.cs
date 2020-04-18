using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public abstract class EngineInstruction
    {
        public abstract object Execute(CompilerState state);

        public static EngineInstruction Create(Element element)
        {
            if (element.Type == ElementType.Text)
            {
                return TextInstruction.Create(element);
            }
            else if (element.Type == ElementType.Block)
            {
                return new BlockEnvironmentInstruction();
            }
            return null;
        }
    }
}
