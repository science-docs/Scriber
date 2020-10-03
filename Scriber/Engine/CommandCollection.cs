using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Engine
{
    public class CommandCollection
    {
        private readonly Dictionary<string, List<Command>> commands = new Dictionary<string, List<Command>>();

        public void Add(Command command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!commands.TryGetValue(command.Name, out var list))
            {
                list = new List<Command>
                {
                    command
                };
                commands[command.Name] = list;
            }
            else
            {
                bool inserted = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (command.Parameters.Count > list[i].Parameters.Count)
                    {
                        inserted = true;
                        list.Insert(i, command);
                        break;
                    }
                }
                if (!inserted)
                {
                    list.Add(command);
                }
            }
        }

        public Command? Find(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            commands.TryGetValue(name, out var command);
            return command.FirstOrDefault();
        }

        public Command? Find(string name, int argumentCount)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (commands.TryGetValue(name, out var list))
            {
                foreach (var command in list)
                {
                    if (command.Parameters.Count <= argumentCount)
                    {
                        return command;
                    }
                }
                return list.FirstOrDefault();
            }
            return null;
        }

        public bool Remove(string name)
        {
            return commands.Remove(name);
        }
    }
}
