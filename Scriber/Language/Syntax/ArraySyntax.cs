namespace Scriber.Language.Syntax
{
    public class ArraySyntax : ListSyntax<ListSyntax>
    {
        public int Arity => Count;
    }
}
