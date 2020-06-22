using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Base class for elements with affixes.
    /// </summary>
    public abstract class RenderingElement : Element, IEvaluated
    {
        /// <summary>
        /// Gets or sets the value that is added before the output of the element carrying the attribute, but only if the element produces output.
        /// </summary>
        [XmlAttribute("prefix")]
        public string? Prefix
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value that is added after the output of the element carrying the attribute, but only if the element produces output.
        /// </summary>
        [XmlAttribute("suffix")]
        public string? Suffix
        {
            get;
            set;
        }

        public abstract bool HasVariableDefined(Interpreter interpreter, Citation citation);

        public void Evaluate(Interpreter interpreter, Citation citation)
        {
            int prefixCount = interpreter.Run.Children.Count;
            EvaluateOverride(interpreter, citation);
            if (interpreter.Run.Children.Count > prefixCount)
            {
                interpreter.Insert(prefixCount, Prefix, this as IFormatting);
                interpreter.Push(Suffix, this as IFormatting);
            }
        }

        public abstract void EvaluateOverride(Interpreter interpreter, Citation citation);

        /// <summary>
        /// Returns all child rendering elements.
        /// </summary>
        /// <returns></returns>
        internal virtual IEnumerable<RenderingElement> GetChildren()
        {
            yield return this;
        }
    }
}
