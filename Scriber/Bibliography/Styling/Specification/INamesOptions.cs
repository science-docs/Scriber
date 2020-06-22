namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Represents an element with names option attributes.
    /// </summary>
    public interface INamesOptions
    {
        /// <summary>
        /// Specifies the delimiter for names of the different name variables (e.g. the semicolon in “Doe, Smith (editors); Johnson (translator)”).
        /// </summary>
        string? Delimiter
        {
            get;
        }
    }
}
