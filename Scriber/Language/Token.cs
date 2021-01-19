using System.Diagnostics;

namespace Scriber.Language
{
    public enum TokenType
    {
        None = 0,
        Text,
        /// <summary>
        /// The @ character. Usually initiates a command.
        /// </summary>
        At,
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
        QuotedRaw,
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
        DoubleCurlyOpen,
        DoubleCurlyClose,
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

        public Token(int index, int line)
        {
            Content = string.Empty;
            Index = index;
            Line = line;
            Type = TokenType.Text;
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
