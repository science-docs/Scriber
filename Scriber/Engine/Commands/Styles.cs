using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Styles
    {
        [Command("SetStyle")]
        public static void SetStyle(string selector, DynamicDictionary style)
        {
        }
    }
}
