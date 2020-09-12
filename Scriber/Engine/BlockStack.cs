using System;
using System.Collections.Generic;

namespace Scriber.Engine
{
    public class BlockStack
    {
        public Block Current => stack.Peek();
        public Block Root { get; }
        public int Count => stack.Count;
        public bool IsRoot => Current == Root;
        
        private readonly Stack<Block> stack = new Stack<Block>();

        public BlockStack()
        {
            Root = Push();
        }

        public Block Push()
        {
            return Push((string?)null);
        }

        public Block Push(string? name)
        {
            Push(new Block(name));
            return Current;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <exception cref="ArgumentNullException"/>
        public void Push(Block block)
        {
            if (block is null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            stack.Push(block);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"/>
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
