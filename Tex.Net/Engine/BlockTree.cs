using System;
using System.Collections.Generic;

namespace Tex.Net.Engine
{
    public class BlockTree
    {
        public Block Current => stack.Peek();

        private readonly Stack<Block> stack = new Stack<Block>();

        public bool IsRoot => stack.Count == 1;
        
        public BlockTree()
        {
            Push();
        }

        public Block Push()
        {
            return Push("{none}");
        }

        public Block Push(string name)
        {
            Push(new Block(name));
            return Current;
        }

        public void Push(Block env)
        {
            stack.Push(env);
        }

        public Block Peek(int i)
        {
            int level = 0;

            foreach (var block in stack)
            {
                if (level++ == i)
                {
                    return block;
                }
            }
            throw new IndexOutOfRangeException();
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
