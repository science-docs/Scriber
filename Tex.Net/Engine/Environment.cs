using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Layout;

namespace Tex.Net.Engine
{
    public class Environment
    {
        public Environment Parent { get; }

        public Unit FontSize { get; set; }

        public Environment(Environment parent)
        {
            Parent = parent;
        }
    }
}
