using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Represents a cs:else conditional branch inside a cs:choose element.
    /// </summary>
    public class ElseElement : Element, IEvaluated
    {
        /// <summary>
        /// Gets or sets the rendered child elements if the condition is met.
        /// </summary>
        [XmlElement("choose", Type = typeof(ChooseElement))]
        [XmlElement("date", Type = typeof(DateElement))]
        [XmlElement("group", Type = typeof(GroupElement))]
        [XmlElement("number", Type = typeof(NumberElement))]
        [XmlElement("names", Type = typeof(NamesElement))]
        [XmlElement("label", Type = typeof(LabelElement))]
        [XmlElement("text", Type = typeof(TextElement))]
        public List<RenderingElement> Children { get; set; } = new List<RenderingElement>();

        public void Evaluate(Interpreter interpreter)
        {
            if (Children != null)
            {
                interpreter.Join(Children.ToArray());
            }
        }

        public bool HasVariableDefined(Interpreter interpreter)
        {
            return Children.Any(e => e.HasVariableDefined(interpreter));
        }

        ///// <summary>
        ///// Compiles this Else element.
        ///// </summary>
        ///// <param name="code"></param>
        //internal virtual void Compile(Scope code)
        //{
        //    // init
        //    code.Append("{{return ");

        //    // array
        //    code.AppendArray("Result", this.Children, (child, scope) => child.Compile(scope));

        //    // done
        //    code.Append(";}}");


        //    //code.Append("{{return new Result[]");
        //    //using (var block = code.AppendBlock(false, ";}"))
        //    //{
        //    //    if (this.Children != null)
        //    //    {
        //    //        // compile children
        //    //        int index = 0;
        //    //        foreach (var child in this.Children.Cast<RenderingElement>())
        //    //        {
        //    //            // compile
        //    //            block.AppendIndent();
        //    //            child.Compile(block);

        //    //            // comma
        //    //            if (index < this.Children.Length - 1)
        //    //            {
        //    //                block.Append(",");
        //    //            }

        //    //            // done
        //    //            block.AppendLineBreak();
        //    //        }
        //    //    }
        //    //}
        //}
    }
}