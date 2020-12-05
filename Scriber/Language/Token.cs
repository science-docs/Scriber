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
        /// <summary>
        /// Double forward slash indicates a single-line comment
        /// </summary>
        DoubleSlash,
        /// <summary>
        /// Indicates the start of a multi-line comment.
        /// </summary>
        SlashAsterisk,
        /// <summary>
        /// Indicates the end of a multi-line comment.
        /// </summary>
        AsteriskSlash,
        Quotation,
        Newline,
        /// <summary>
        /// Whitespace character. Accounts for every whitespace character like tabulator.
        /// </summary>
        Whitespace,
        Percent,
        Currency,
        /// <summary>
        /// Underscore is used for subscript text. Replaced by the @Subscript() command during parsing.
        /// </summary>
        Underscore,
        ParenthesesOpen,
        ParenthesesClose,
        CurlyOpen,
        CurlyClose,
        BracketOpen,
        BracketClose,
        Tilde,
        Colon,
        /// <summary>
        /// Used as a delimiter for command arguments, array items and object fields.
        /// </summary>
        Semicolon,
        /// <summary>
        /// The ^ character. Also known as circumflex. Used for superscript text. Replaced by the @Superscript() command during parsing.
        /// </summary>
        Caret
    }

    [DebuggerDisplay("{Content}")]
    public class Token
    {
        public string Content { get; set; }
        public TokenType Type { get; set; }
        public TextSpan Span => new TextSpan(Index, Length, Line);
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
