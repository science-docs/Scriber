using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public class EnvironmentTree
    {
        public Environment Current { get; private set; }
        public Environment Root { get; private set; }

        public bool IsRoot => Current == Root;
        
        public Environment Push()
        {
            if (Root == null)
            {
                Root = new Environment(null);
                Current = Root;
            }
            else
            {
                Current = new Environment(Current);
            }

            return Current;
        }

        public void Pop()
        {
            if (Current != Root)
            {
                Current = Current.Parent;
            }
        }
    }
}
