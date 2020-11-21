namespace Scriber.Language.Syntax
{
    public class ArraySyntax : ListSyntax
    {
        public int Arity => Children.Count;
    }
}
