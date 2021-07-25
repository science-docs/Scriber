using Scriber.Engine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scriber.Autocomplete
{
    public class FileProposalProvider : IProposalProvider
    {
        public string Filter { get; set; }

        public FileProposalProvider() : this("*")
        {
        }

        public FileProposalProvider(string filter)
        {
            Filter = filter;
        }

        public IEnumerable<Proposal> Propose(CompilerState state)
        {
            var currentDir = state.FileSystem.Directory.GetCurrentDirectory();
            var subFiles = state.FileSystem.Directory.GetFiles(currentDir, Filter, SearchOption.AllDirectories);
            
            foreach (var file in subFiles.OrderBy(e => e))
            {
                var relativePath = state.FileSystem.Path.GetRelativePath(currentDir, file);
                yield return new Proposal(relativePath) { Type = ProposalType.File };
            }
        }

        public static string BuildFilter(params string[] extensions)
        {
            return string.Join('|', extensions.Select(e => $"*.{e}"));
        }
    }
}
