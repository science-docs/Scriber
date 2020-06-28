using System;
using System.Collections.Generic;

namespace Scriber.Engine
{
    public class CommandCollection
    {
        private readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public void Add(Command command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            commands[command.Name] = command;
        }

        public Command? Find(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            commands.TryGetValue(name, out var command);
            return command;
        }
    }
}
