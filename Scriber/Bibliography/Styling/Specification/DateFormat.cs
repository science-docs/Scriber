using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Specifies the format of a localized date element.
    /// </summary>
    public enum DateFormat
    {
        /// <summary>
        /// “numeric” (for fully numeric formats, e.g. “12-15-2005”).
        /// </summary>
        [XmlEnum("numeric")]
        Numeric,
        /// <summary>
        /// “text” (for formats with a non-numeric month, e.g. “December 15, 2005”).
        /// </summary>
        [XmlEnum("text")]
        Text
    }
}
