using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Macros allow formatting instructions to be reused, keeping styles compact and maintainable.
    /// The use of macros can improve style readability, compactness and maintainability. It is recommended to keep 
    /// the contents of cs:citation and cs:bibliography compact and agnostic of item types (e.g. books, journal 
    /// articles, etc.) by depending on macro calls. To allow for easy reuse of macros in other styles, it is 
    /// recommended to use common macro names.
    /// </summary>
    public class MacroElement : Element, IEvaluated
    {
        /// <summary>
        /// The name of the macro, which is also referenced by the cs:text and cs:sort elements.
        /// </summary>
        [XmlAttribute("name")]
        public string? Name
        {
            get;
            set;
        }

        /// <summary>
        /// The child elements to be rendered.
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

        public void Evaluate(Interpreter interpreter, Citation citation)
        {
            if (Children != null)
            {
                interpreter.Join(citation, Children.ToArray());
            }
        }

        public bool HasVariableDefined(Interpreter interpreter, Citation citation)
        {
            if (Children != null)
            {
                return Children.Any(e => e.HasVariableDefined(interpreter, citation));
            }
            return false;
        }

        ///// <summary>
        ///// Compiles this macro element.
        ///// </summary>
        ///// <param name="code"></param>
        //internal void Compile(Scope code)
        //{
        //    // init
        //    code.AppendComment("macro: '{0}'", this.Name);

        //    // invoke
        //    code.AppendIndent();
        //    using (var method = code.AppendMethodInvoke("return this.RenderMacro", this))
        //    {
        //        // parameters
        //        method.AddElement(this);
        //        method.AddContextAndParameters();

        //        // children
        //        using (var lambda = method.AddLambdaExpression(false))
        //        {
        //            lambda.AppendArray("Result", this.Children.Cast<RenderingElement>(), (child, scope) => child.Compile(scope), scope => scope.Append("null"));
        //        }
        //    }

        //    // done
        //    code.Append(";");
        //    code.AppendLineBreak();
        //}
    }
}