using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public class EnvironmentTree
    {
        public Environment Current { get; private set; }
        public Environment Root { get; private set; }
        
        public void Push()
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
