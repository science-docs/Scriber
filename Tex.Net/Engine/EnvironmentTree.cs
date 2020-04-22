using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Engine
{
    public class EnvironmentTree
    {
        public Environment Current => stack.Peek();

        private readonly Stack<Environment> stack = new Stack<Environment>();

        public bool IsRoot => stack.Count == 1;
        
        public EnvironmentTree()
        {
            Push();
        }

        public Environment Push()
        {
            return Push("{none}");
        }

        public Environment Push(string name)
        {
            stack.Push(new Environment(name));
            return Current;
        }

        public void Pop()
        {
            if (!IsRoot)
            {
                stack.Pop();
            }
        }
    }
}
