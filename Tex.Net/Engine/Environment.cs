using System.Collections.Generic;
using Tex.Net.Layout;
using Tex.Net.Text;

namespace Tex.Net.Engine
{
    public class Environment
    {
        public Environment Parent { get; }

        public Unit FontSize { get; set; }

        // TODO: Change this into a font family
        public Font Font { get; set; }

        public List<object> Objects { get; } = new List<object>();

        public Environment(Environment parent)
        {
            Parent = parent;
        }
    }
}
