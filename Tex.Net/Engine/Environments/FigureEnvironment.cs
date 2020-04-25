using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine.Environments
{
    [Package]
    public static class FigureEnvironment
    {
        [Environment("figure")]
        public static object Figure(CompilerState state, object[] content)
        {
            return content;
        }
    }
}
