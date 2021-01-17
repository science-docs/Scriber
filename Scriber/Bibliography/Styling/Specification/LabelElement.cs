using Scriber.Util;
using System;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// The cs:label rendering element outputs the term matching the variable selected with the required variable 
    /// attribute, which must be set to “locator, “page”, or one of the number variables. The term is only rendered
    /// if the selected variable is non-empty. 
    /// </summary>
    public class LabelElement : FormattingElement, IStripPeriods
    {
        /// <summary>
        /// The term/variable to be rendered.
        /// </summary>
        [XmlAttribute("variable")]
        public string? Variable
        {
            get;
            set;
        }

        /// <summary>
        /// Selects the format of the term.
        /// </summary>
        [XmlAttribute("form")]
        public TermFormat Format
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
        /// Sets pluralization of the term: contextual (default), always or never.
        /// </summary>
        [XmlAttribute("plural")]
        public LabelPluralization Plurilization
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'plural' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool PlurilizationSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When set to “true” (“false” is the default), any periods in the rendered text are removed.
        /// </summary>
        [XmlAttribute("strip-periods")]
        public bool StripPeriods
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'strip-periods' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool StripPeriodsSpecified
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

        public override void EvaluateOverride(Interpreter interpreter)
        {
            if (Variable == null || !EnumUtility.TryParseEnum<TermName>(Variable.ToPascalCase(), out var term))
            {
                throw new Exception();
            }

            var value = interpreter.Variable(Variable);

            if (value is INumberVariable number)
            {
                var single = Plurilization switch
                {
                    LabelPluralization.Always => false,
                    LabelPluralization.Never => true,
                    LabelPluralization.Contextual => number.Max == number.Max,
                    _ => true
                };

                var termValue = interpreter.Locale.GetTerm(term, Format, single);
                interpreter.Push(termValue, this);
            }
        }

        public override bool HasVariableDefined(Interpreter interpreter)
        {
            if (Variable != null)
            {
                return interpreter.Variable(Variable) != null;
            }
            return false;
        }
    }
}