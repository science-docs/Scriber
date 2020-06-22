﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Specifies the gender of a term.
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Masculine.
        /// </summary>
        [XmlEnum("masculine")]
        Masculine,
        /// <summary>
        /// Feminine.
        /// </summary>
        [XmlEnum("feminine")]
        Feminine,
    }
}
