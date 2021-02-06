using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public List<KeyElement> Keys
        {
            get;
            set;
        } = new List<KeyElement>();

        public IEnumerable<Citation> Sort(Interpreter interpreter, IEnumerable<Citation> citations)
        {
            if (Keys == null || Keys.Count == 0)
            {
                return citations;
            }

            IOrderedEnumerable<Citation>? sorted = null;
            
            foreach (var key in Keys)
            {
                var orderFunction = OrderValue(interpreter, key);
                var comparer = new SortComparer(key.SortOrder);

                if (sorted == null)
                {
                    sorted = citations.OrderBy(orderFunction, comparer);
                }
                else
                {
                    sorted = sorted.ThenBy(orderFunction, comparer);
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
                    interpreter.Citation = citation;
                    return interpreter.Variable(key.Variable)?.ToString();
                };
            }
            else if (key.Macro != null)
            {
                return citation =>
                {
                    interpreter.Clear();
                    interpreter.Citation = citation;
                    var macroElement = interpreter.StyleFile.FindMacro(key.Macro);
                    if (macroElement != null)
                    {
                        macroElement.Evaluate(interpreter);
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

        private class SortComparer : IComparer<string?>
        {
            public SortOrder Order { get; set; }

            public SortComparer(SortOrder order)
            {
                Order = order;
            }

            public int Compare([AllowNull] string? x, [AllowNull] string? y)
            {
                var multiplier = Order == SortOrder.Ascending ? 1 : -1;
                if (x is null && y is null)
                {
                    return 0;
                }
                else if (y is null)
                {
                    return -1 * multiplier;
                }
                else if (x is null)
                {
                    return 1 * multiplier;
                }
                else
                {
                    return x.CompareTo(y) * multiplier;
                }
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