namespace Scriber.Language.Syntax
{
    public class ArraySyntax : ListSyntax<ListSyntax<ListSyntax>>
    {
        public int Arity => Children.Count;
    }
}
