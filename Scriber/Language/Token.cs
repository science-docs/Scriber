using System.Diagnostics;

namespace Scriber.Language
{
    public enum TokenType
    {
        Text = 0,
        /// <summary>
        /// The @ character. Usually initiates a command.
        /// </summary>
        At,
        /// <summary>
        /// Special backslash character used for escaping and hyphenation
        /// </summary>
        Backslash,
        Quotation,
        Newline,
        Ampersand,
        Percent,
        Currency,
        Underscore,
        ParenthesesOpen,
        ParenthesesClose,
        CurlyOpen,
        CurlyClose,
        BracketOpen,
        BracketClose,
        Tilde,
        Colon,
        Comma,
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
        public int Line { get; set; }

        public Token(string content, int index, int line)
        {
            Content = content;
            Index = index;
            Line = line;
            Type = TokenType.Text;
        }

        public Token(TokenType type, int index, int line)
        {
            Type = type;
            Index = index;
            Line = line;
            Content = null ?? string.Empty;
        }

        public Token(TokenType type, int index, int line, string content)
        {
            Type = type;
            Index = index;
            Line = line;
            Content = content;
        }
    }
}
