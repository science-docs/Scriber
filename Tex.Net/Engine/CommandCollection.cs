using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine
{
    public static class CommandCollection
    {
        private static readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public static void Add(Command command)
        {
            commands[command.Name] = command;
        }

        public static Command? Find(string name)
        {
            commands.TryGetValue(name, out var command);
            return command;
        }
    }
}
