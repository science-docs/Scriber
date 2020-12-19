using Scriber.Engine;
using Scriber.Logging;
using System.IO.Abstractions;

namespace Scriber
{
    public class Context
    {
        public IFileSystem FileSystem { get; }
        public ResourceSet ResourceSet { get; }
        public Logger Logger { get; } = new Logger();
        public ConverterCollection Converters { get; } = new ConverterCollection();
        public CommandCollection Commands { get; } = new CommandCollection();
        public VariableCollection Variables { get; } = new VariableCollection();
        public bool FailOnError { get; set; }

        public Context() : this(null)
        {

        }

        public Context(IFileSystem? fileSystem)
        {
            FileSystem = fileSystem ?? new FileSystem();
            ResourceSet = new ResourceSet(FileSystem);
        }
    }
}
