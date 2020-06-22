using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Specifies the font variant (normal or small-caps).
    /// </summary>
    public enum FontVariant
    {
        /// <summary>
        /// “normal” (default).
        /// </summary>
        [XmlEnum("normal")]
        Normal,
        /// <summary>
        /// “small-caps”.
        /// </summary>
        [XmlEnum("small-caps")]
        SmallCaps
    }
}
