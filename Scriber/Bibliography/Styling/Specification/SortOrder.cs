using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Describes a sort order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// Ascending.
        /// </summary>
        [XmlEnum("ascending")]
        Ascending,
        /// <summary>
        /// Descending.
        /// </summary>
        [XmlEnum("descending")]
        Descending
    }
}
