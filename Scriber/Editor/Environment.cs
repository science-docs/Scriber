using Scriber.Autocomplete;
using Scriber.Engine;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading;

namespace Scriber.Editor
{
    public class Environment
    {
        public Context Context { get; }
        public Completion Completion { get; }
        public CompilerState CurrentState { get; private set; }

        private readonly Dictionary<Uri, EditorDocument> documents = new();

        public Environment() : this(null)
        {
        }

        public Environment(IFileSystem? fileSystem)
        {
            Context = new Context(fileSystem);
            Context.ResourceManager.Interject = InterjectResourceManager;
            var reflectionLoader = new ReflectionLoader();
            reflectionLoader.Discover(Context);
            CurrentState = new CompilerState(Context, CancellationToken.None);
            Completion = new Completion(this);
        }

        public EditorDocument GetDocument(Uri uri)
        {
            if (!documents.TryGetValue(uri, out var document))
            {
                document = Open(uri, false);
            }
            return document;
        }

        public EditorDocument Open(Uri uri)
        {
            return Open(uri, true);
        }

        private EditorDocument Open(Uri uri, bool setOpen)
        {
            var resource = Context.ResourceManager.Get(uri);
            var document = new EditorDocument(uri, resource.GetContentAsString())
            {
                Open = setOpen
            };
            return documents[uri] = document;
        }

        public void Close(Uri uri)
        {
            if (documents.TryGetValue(uri, out var document))
            {
                document.Open = false;
            }
        }

        public void Change(Uri uri, IEnumerable<DocumentChange> changes)
        {
            if (documents.TryGetValue(uri, out var document))
            {
                foreach (var change in changes)
                {
                    document.Change(change);
                }
            }
        }

        public CompilerState Compile(Uri uri, CancellationToken cancellationToken)
        {
            if (!documents.TryGetValue(uri, out var document))
            {
                document = Open(uri, false);
            }
            Context.ResourceManager.PushResource(new Resource(uri, ""));
            CurrentState = Compiler.Compile(Context, document.ParserResult.Nodes, cancellationToken);
            Context.ResourceManager.PopResource();
            UpdateCompilerIssues();
            RemoveUnecessaryDocuments();
            return CurrentState;
        }

        private Resource? InterjectResourceManager(Uri uri)
        {
            if (documents.TryGetValue(uri, out var document))
            {
                return new Resource(uri, document.Content);
            }
            else
            {
                return null;
            }
        }

        private void UpdateCompilerIssues()
        {
            foreach (var document in documents.Values)
            {
                document.Issues.RemoveAll(e => e.Code == "compiler");
            }

            foreach (var issue in CurrentState.Issues)
            {
                var uri = issue.Origin.Resource.Uri;
                if (!documents.ContainsKey(uri))
                {
                    Open(uri, false);
                }

                var document = documents[uri];
                document.Issues.Add(Issue.FromCompilerIssue(issue));
            }
        }

        private void RemoveUnecessaryDocuments()
        {
            var toRemove = new List<Uri>();
            foreach (var (uri, document) in documents)
            {
                if (!document.Open && document.Issues.Count == 0)
                {
                    toRemove.Add(uri);
                }
            }
            foreach (var remove in toRemove)
            {
                documents.Remove(remove);
            }
        }
    }
}
