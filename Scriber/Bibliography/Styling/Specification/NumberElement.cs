﻿using Scriber.Bibliography.Styling.Formatting;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// The cs:number rendering element outputs the number variable selected with the required variable attribute. A number variable
    /// is rendered with cs:number and only contains numeric content (as determined by the rules for is-numeric), the number(s) are
    /// extracted. Variable content is rendered “as is” when the variable contains any non-numeric content (e.g. “Special edition”).
    /// </summary>
    public class NumberElement : FormattingElement
    {
        /// <summary>
        /// The variable containing the (numeric) content.
        /// </summary>
        [XmlAttribute("variable")]
        public string? Variable
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the number format.
        /// </summary>
        [XmlAttribute("form")]
        public NumberFormat Format
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

        public override void EvaluateOverride(Interpreter interpreter, Citation citation)
        {
            if (Variable == null)
            {
                throw new System.Exception();
            }

            var value = citation[Variable];

            var text = value switch
            {
                ITextVariable textVariable => textVariable.Value,
                INumberVariable numberVariable => RenderNumber(interpreter, numberVariable),
                _ => null
            };

            if (text != null)
            {
                interpreter.Push(text, this);
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

        private string RenderNumber(Interpreter interpreter, INumberVariable number)
        {
            if (number.Min != number.Max)
            {
                var min = interpreter.Locale.FormatNumber(number.Min, Format);
                var max = interpreter.Locale.FormatNumber(number.Min, Format);

                var spaceBefore = number.Separator == '&';
                var spaceAfter = number.Separator == ',' || number.Separator == '&';

                return $"{min}{(spaceBefore ? " " : "")}{number.Separator}{(spaceAfter ? " " : "")}{max}";
            }
            else
            {
                return interpreter.Locale.FormatNumber(number.Min, Format);
            }
        }

        ///// <summary>
        ///// Compiles this Number element.
        ///// </summary>
        ///// <param name="code"></param>
        //internal override void Compile(Scope code)
        //{
        //    // validate
        //    if (this.DisplaySpecified)
        //    {
        //        throw new FeatureNotSupportedException("display attribute");
        //    }

        //    // invoke
        //    using (var method = code.AppendMethodInvoke("this.RenderNumber", this))
        //    {
        //        // parameters
        //        method.AddElement(this);
        //        method.AddLiteral(this.Variable.ToLower());
        //        method.AddLiteral(Utility.GetXmlEnum<TermName>(this.Variable));
        //        method.AddLiteral(this.FormatSpecified ? this.Format : NumberFormat.Numeric);
        //        method.AddLiteral(this.Prefix);
        //        method.AddLiteral(this.Suffix);
        //        method.AddLiteral(this.TextCaseSpecified ? (object)this.TextCase : null);
        //        method.AddContextAndParameters();
        //    }
        //}
    }
}
