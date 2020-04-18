using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net
{
    public class CommandCollection
    {
        private readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public Command this[string name]
        {
            get => commands.TryGetValue(name, out var command) ? command : null;
            set => commands.TryAdd(name, value);
        }

        public CommandCollection()
        {

        }
    }
}
