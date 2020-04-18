using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public static class Compiler
    {
        public static CompilerResult Compile(Element element)
        {
            var state = new CompilerState();

            Execute(state, element);

            var result = new CompilerResult(state.Document);
            result.Issues.AddRange(state.Issues);
            return result;
        }

        private static void Execute(CompilerState state, Element element)
        {
            if (element.Inline != null)
            {
                Execute(state, element.Inline);
            }

            state.Execute(element);

            if (element.NextSibling != null)
            {
                Execute(state, element.NextSibling);
            }
        }
    }
}
