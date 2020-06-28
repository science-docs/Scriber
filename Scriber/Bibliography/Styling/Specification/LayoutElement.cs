using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    public class LayoutElement : FormattingElement
    {
        /// <summary>
        /// The delimiter attribute delimits non-empty pieces of output.
        /// </summary>
        [XmlAttribute("delimiter")]
        public string? Delimiter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rendering elements that make up this layout.
        /// </summary>
        [XmlElement("choose", Type = typeof(ChooseElement))]
        [XmlElement("date", Type = typeof(DateElement))]
        [XmlElement("group", Type = typeof(GroupElement))]
        [XmlElement("number", Type = typeof(NumberElement))]
        [XmlElement("names", Type = typeof(NamesElement))]
        [XmlElement("label", Type = typeof(LabelElement))]
        [XmlElement("text", Type = typeof(TextElement))]
        public List<RenderingElement> Children
        {
            get;
            set;
        } = new List<RenderingElement>();

        public override void EvaluateOverride(Interpreter interpreter, Citation citation)
        {
            if (Children != null)
            {
                interpreter.Join(citation, this, Delimiter, Children.ToArray());
            }
        }

        public override bool HasVariableDefined(Interpreter interpreter, Citation citation)
        {
            return false;
        }

        /// <summary>
        /// Returns all child elements.
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<RenderingElement> GetChildren()
        {
            return base.GetChildren().Concat(this.Children.SelectMany(x => x.GetChildren()));
        }
    }
}