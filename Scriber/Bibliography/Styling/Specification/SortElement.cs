using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Specifies the sorting order of cites within citations, and bibliographic entries within the bibliography respectively.
    /// </summary>
    public class SortElement : Element
    {
        /// <summary>
        /// The list of sort keys.
        /// </summary>
        [XmlElement("key")]
        public KeyElement[]? Keys
        {
            get;
            set;
        }

        public IEnumerable<Citation> Sort(Interpreter interpreter, IEnumerable<Citation> citations)
        {
            if (Keys == null)
            {
                return citations;
            }

            IOrderedEnumerable<Citation>? sorted = null;
            
            foreach (var key in Keys)
            {
                var orderFunction = OrderValue(interpreter, key);

                if (key.SortOrder == SortOrder.Ascending)
                {
                    if (sorted == null)
                    {
                        sorted = citations.OrderBy(orderFunction);
                    }
                    else
                    {
                        sorted = sorted.ThenBy(orderFunction);
                    }
                }
                else
                {
                    if (sorted == null)
                    {
                        sorted = citations.OrderByDescending(orderFunction);
                    }
                    else
                    {
                        sorted = sorted.ThenByDescending(orderFunction);
                    }
                }
            }
            interpreter.Clear();

            return sorted ?? citations;
        }

        private Func<Citation, string?> OrderValue(Interpreter interpreter, KeyElement key)
        {
            if (key.Variable != null)
            {
                return citation =>
                {
                    return citation[key.Variable]?.ToString();
                };
            }
            else if (key.Macro != null)
            {
                return citation =>
                {
                    interpreter.Clear();
                    var macroElement = interpreter.StyleFile.FindMacro(key.Macro);
                    if (macroElement != null)
                    {
                        macroElement.Evaluate(interpreter, citation);
                        var text = interpreter.Run.ToPlainText();
                        return text;
                    }
                    else
                    {
                        return null;
                    }
                };
            }
            else
            {
                return citation => null;
            }
        }

        ///// <summary>
        ///// Compiles this Sort element.
        ///// </summary>
        ///// <param name="code"></param>
        //internal void Compile(LambdaExpression code)
        //{
        //    // invoke
        //    using (var method = code.AppendMethodInvoke("this.RenderSort", this))
        //    {
        //        // parameters
        //        method.AddElement(this);
        //        method.AddContextAndParameters();

        //        // children
        //        using (var lambda = method.AddLambdaExpression(false))
        //        {
        //            lambda.AppendArray("string", this.Keys, (key, scope) => key.Compile(scope));
        //        }
        //    }
        //}
    }
}