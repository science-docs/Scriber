using Scriber.Bibliography.Styling.Renderer;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// The cs:date rendering element outputs the date selected from the list of date variables with the required variable
    /// attribute. A date can be rendered in either a localized or non-localized format.
    /// </summary>
    public class DateElement : FormattingElement
    {
        /// <summary>
        /// Specifies which date variable will be rendered.
        /// </summary>
        [XmlAttribute("variable")]
        public string? Variable
        {
            get;
            set;
        }

        /// <summary>
        /// Localized date formats are selected with the optional form attribute, which must set to either “numeric” (for
        /// fully numeric formats, e.g. “12-15-2005”), or “text” (for formats with a non-numeric month, e.g. “December 15, 2005”). 
        /// </summary>
        [XmlAttribute("form")]
        public DateFormat Format
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'form' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool FormatSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// The date-parts attribute may be used to show fewer date parts.
        /// </summary>
        [XmlAttribute("date-parts")]
        public DatePrecision Precision
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'date-parts' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool PrecisionSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When specified, converts the rendered text to the given case.
        /// </summary>
        [XmlAttribute("text-case")]
        public TextCase TextCase
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'text-case' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool TextCaseSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// For date formats with the form attribute specified, the attributes set on these elements override those specified for
        /// the localized date formats. In the absence of the form attribute, cs:date describes a self-contained non-localized date
        /// format. In this case, the date format is constructed using cs:date-part child elements. With a required name attribute set
        /// to either day, month or year, the order of these elements reflects the display order of respectively the day, month, and year.
        /// </summary>
        [XmlElement("date-part")]
        public List<DatePartElement> DateParts
        {
            get;
            set;
        } = new List<DatePartElement>();

        /// <summary>
        /// The display attribute (similar the “display” property in CSS) may be used to structure individual bibliographic entries
        /// into one or more text blocks. If used, all rendering elements should be under the control of a display attribute. 
        /// </summary>
        [XmlAttribute("display")]
        public Display Display
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'display' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DisplaySpecified
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

        public override void EvaluateOverride(Interpreter interpreter, Citation citation)
        {
            if (Variable == null)
            {
                throw new ArgumentNullException(nameof(Variable));
            }

            var value = citation[Variable];

            if (FormatSpecified)
            {
                DateRenderer.RenderLocalizedDate(interpreter, this, value, Delimiter, Format, Precision, DateParts);
            }
            else
            {
                DateRenderer.RenderNonLocalizedDate(interpreter, this, value, Delimiter, Precision, DateParts);
            }
        }

        public override bool HasVariableDefined(Interpreter interpreter, Citation citation)
        {
            if (Variable != null)
            {
                return citation[Variable] != null;
            }
            return false;
        }
    }
}
