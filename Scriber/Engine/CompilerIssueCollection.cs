using Scriber.Language;
using System.Collections.ObjectModel;

namespace Scriber.Engine
{
    public class CompilerIssueCollection : ObservableCollection<CompilerIssue>
    {
        public void Add(Element origin, CompilerIssueType type, string message)
        {
            Add(new CompilerIssue(origin, type, message));
        }

        public void Log(Element origin, string message)
        {
            Add(origin, CompilerIssueType.Log, message);
        }
    }
}
