namespace Scriber.Language.Syntax
{
    public class CommentSyntax : SyntaxNode
    {
        public bool Multiline { get; set; } = false;
        public string Comment { get; set; } = string.Empty;
    }
}
