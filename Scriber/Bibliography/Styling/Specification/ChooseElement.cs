using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// The cs:choose rendering element allows for conditional rendering of rendering elements.
    /// </summary>
    public class ChooseElement : RenderingElement
    {
        /// <summary>
        /// Represents the first, 'if', conditional branch.
        /// </summary>
        [XmlElement("if")]
        public IfElement? If
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the subsequent, 'else if' conditional branches.
        /// </summary>
        [XmlElement("else-if")]
        public List<IfElement> ElseIf
        {
            get;
            set;
        } = new List<IfElement>();
        /// <summary>
        /// Represents the last, 'else' conditional branch.
        /// </summary>
        [XmlElement("else")]
        public ElseElement? Else
        {
            get;
            set;
        }

        public override void EvaluateOverride(Interpreter interpreter)
        {
            ElseElement? renderingElement = null;

            if (If != null && If.Matches(interpreter))
            {
                renderingElement = If;
            }
            else if (ElseIf != null)
            {
                foreach (var elseIf in ElseIf)
                {
                    if (elseIf.Matches(interpreter))
                    {
                        renderingElement = elseIf;
                        break;
                    }
                }
            }

            if (renderingElement == null)
            {
                renderingElement = Else;
            }

            if (renderingElement != null)
            {
                renderingElement.Evaluate(interpreter);
            }
        }

        public override bool HasVariableDefined(Interpreter interpreter)
        {
            return (If != null && If.HasVariableDefined(interpreter)) || (ElseIf != null && ElseIf.Any(e => e.HasVariableDefined(interpreter))) || (Else != null && Else.HasVariableDefined(interpreter));
        }
    }
}
