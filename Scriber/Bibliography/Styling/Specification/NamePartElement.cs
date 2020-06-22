using Scriber.Bibliography.Styling.Formatting;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    public class NamePartElement : FormattingElement
    {
        [XmlAttribute("name")]
        public NamePartName Name
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

        public override void EvaluateOverride(Interpreter interpreter, Citation citation)
        {
            throw new System.NotSupportedException();
        }

        public override bool HasVariableDefined(Interpreter interpreter, Citation citation)
        {
            return false;
        }

        //internal void Compile(MethodInvoke method)
        //{
        //    // name
        //    method.AddElement(this);
        //    method.AddCode(method.ParameterName);
        //    method.AddLiteral(this.TextCaseSpecified ? (object)this.TextCase : null);
        //    method.AddLiteral(this.Prefix);
        //    method.AddLiteral(this.Suffix);
        //    method.AddDefaultParameters();
        //}
    }
}
