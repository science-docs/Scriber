using Scriber.Bibliography.Styling.Formatting;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    public class NameElement : FormattingElement, INameOptions
    {
        [XmlElement("name-part")]
        public List<NamePartElement> NameParts
        {
            get;
            set;
        } = new List<NamePartElement>();

        [XmlAttribute("and")]
        public And And
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool AndSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// The delimiter attribute delimits non-empty pieces of output.
        /// </summary>
        [XmlAttribute("delimiter")]
        public string? Delimiter
        {
            get;
            set;
        }

        [XmlAttribute("delimiter-precedes-et-al")]
        public DelimiterBehavior DelimiterPrecedesEtAl
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool DelimiterPrecedesEtAlSpecified
        {
            get;
            set;
        }

        [XmlAttribute("delimiter-precedes-last")]
        public DelimiterBehavior DelimiterPrecedesLast
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool DelimiterPrecedesLastSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-min")]
        public int EtAlMin
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlMinSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-use-first")]
        public int EtAlUseFirst
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlUseFirstSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-subsequent-min")]
        public int EtAlSubsequentMin
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlSubsequentMinSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-subsequent-use-first")]
        public int EtAlSubsequentUseFirst
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlSubsequentUseFirstSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-use-last")]
        public bool EtAlUseLast
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlUseLastSpecified
        {
            get;
            set;
        }

        [XmlAttribute("form")]
        public NameFormat Format
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool FormatSpecified
        {
            get;
            set;
        }

        [XmlAttribute("initialize")]
        public bool Initialize
        {
            get;
            set;
        } = true;
        [XmlIgnore]
        public bool InitializeSpecified
        {
            get;
            set;
        }

        [XmlAttribute("initialize-with")]
        public string? InitializeWith
        {
            get;
            set;
        }

        [XmlAttribute("name-as-sort-order")]
        public NameSortOptions NameAsSortOrder
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool NameAsSortOrderSpecified
        {
            get;
            set;
        }

        [XmlAttribute("sort-separator")]
        public string? SortSeparator
        {
            get;
            set;
        }

        public override void EvaluateOverride(Interpreter interpreter, Citation citation)
        {
            throw new System.NotSupportedException();
        }

        public override bool HasVariableDefined(Interpreter interpreter, Citation citation)
        {
            return false;
        }
    }
}