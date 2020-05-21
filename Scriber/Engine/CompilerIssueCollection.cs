using Scriber.Language;
using System;
using System.Collections.ObjectModel;

namespace Scriber.Engine
{
    public class CompilerIssueCollection : ObservableCollection<CompilerIssue>
    {
        public void Add(Element origin, CompilerIssueType type, string message)
        {
            Add(origin, type, message, null);
        }

        public void Add(Element origin, CompilerIssueType type, string message, Exception? innerException)
        {
            Add(new CompilerIssue(origin, type, message, innerException));
        }

        public void Log(Element origin, string message)
        {
            Add(origin, CompilerIssueType.Log, message);
        }
    }
}
