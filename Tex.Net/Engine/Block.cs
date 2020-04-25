using System.Collections.Generic;

namespace Tex.Net.Engine
{
    public class Block
    {
        /// <summary>
        /// The name of the block.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List of objects that the current block contains.
        /// </summary>
        public List<object> Objects { get; } = new List<object>();
        /// <summary>
        /// List of arguments that where supplied to the environment begin command.
        /// </summary>
        public List<object> Arguments { get; } = new List<object>();

        public Environment Environment { get; internal set; }

        public Block(string name)
        {
            Name = name;
        }
    }
}
