﻿using Scriber.Language.Syntax;
using System;
using System.Collections.ObjectModel;

namespace Scriber.Language
{
    public class ParserIssueCollection : ObservableCollection<ParserIssue>
    {
        public void Add(Token origin, ParserIssueType type, string message)
        {
            Add(new ParserIssue(origin, type, message, null));
        }

        public void Add(SyntaxNode origin, ParserIssueType type, string message)
        {
            Add(origin, type, message, null);
        }

        public void Add(SyntaxNode origin, ParserIssueType type, string message, Exception? innerException)
        {
            Add(new ParserIssue(origin, type, message, innerException));
        }

        public void Log(SyntaxNode origin, string message)
        {
            Add(origin, ParserIssueType.Log, message);
        }
    }
}
