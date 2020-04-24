using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine.Commands
{
    [Package]
    public static class Layouts
    {
        [Command("setlength")]
        public static void SetLength(CompilerState state, string lengthName, string length)
        {
            var l = double.Parse(length);
            state.Document.Variables[DocumentVariables.Length][lengthName].SetValue(l);
        }
    }
}
