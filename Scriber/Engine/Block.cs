using System.Collections.Generic;

namespace Scriber.Engine
{
    public class Block
    {
        /// <summary>
        /// The name of the block.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// List of objects that the current block contains.
        /// </summary>
        public List<Argument> Objects { get; } = new List<Argument>();

        public Block(string? name)
        {
            Name = name;
        }
    }
}
