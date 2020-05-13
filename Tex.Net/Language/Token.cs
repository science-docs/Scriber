using System.Diagnostics;

namespace Tex.Net.Language
{
    public enum TokenType
    {
        Text = 0,
        /// <summary>
        /// The \ character. Usually initiates a command.
        /// </summary>
        Backslash = 1,
        Newline,
        Ampersand,
        Percent,
        Currency,
        Underscore,
        CurlyOpen,
        CurlyClose,
        BracketOpen,
        BracketClose,
        Tilde,
        /// <summary>
        /// The ^ character. Also known as circumflex.
        /// </summary>
        Caret
    }

    [DebuggerDisplay("{Content}")]
    public class Token
    {
        public string Content { get; set; }
        public TokenType Type { get; set; }
        public int Index { get; set; }
        public int Length => Content.Length;

        public Token(string content, int index)
        {
            Content = content;
            Index = index;
            Type = TokenType.Text;
        }

        public Token(TokenType type, int index)
        {
            Type = type;
            Index = index;
            Content = null ?? string.Empty;
        }

        public Token(TokenType type, int index, string content)
        {
            Type = type;
            Index = index;
            Content = content;
        }
    }
}
