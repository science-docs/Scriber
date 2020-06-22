using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Scriber.Bibliography.BibTex
{
    public class BibEntry
    {
        #region Private Field
        /// <summary>
        /// Entry's type
        /// </summary>
        private EntryType _type;

        /// <summary>
        /// Store all tags
        /// </summary>
        private readonly Dictionary<string, string?> _tags = new Dictionary<string, string?>();
        #endregion

        #region Public Property

        public List<BibEntryItem> Items { get; } = new List<BibEntryItem>();

        public int SourcePosition { get; set; }

        public int SourceLength { get; set; }

        public string? Abstract
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Address
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Annote
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Author
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Booktitle
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Chapter
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Crossref
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Edition
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Editor
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Howpublished
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Institution
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Journal
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Mouth
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Note
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Number
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Organization
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Pages
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Publisher
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? School
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Series
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Title
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Volume
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Year
        {
            get { return Get(); }
            set { Set(value); }
        }

        /// <summary>
        /// Entry's type
        /// </summary>
        public string? Type
        {
            get
            {
                return Enum.GetName(typeof(EntryType), _type);
            }
            set
            {
                if (Enum.TryParse<EntryType>(value, true, out var result))
                {
                    _type = result;
                }
            }
        }

        /// <summary>
        /// Entry's key
        /// </summary>
        public string Key { get; set; } = string.Empty;
        #endregion

        #region Public Method
        /// <summary>
        /// To BibTeX entry
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var bib = new StringBuilder("@");
            bib.Append(Type);
            bib.Append('{');
            bib.Append(Key);
            bib.Append(",");
            bib.Append(Config.LineFeed);

            foreach (var tag in _tags)
            {
                bib.Append(Config.Retract);
                bib.Append(tag.Key);
                bib.Append(" = {");
                bib.Append(tag.Value);
                bib.Append("},");
                bib.Append(Config.LineFeed);
            }

            bib.Append("}");

            return bib.ToString();
        }
        #endregion

        #region Public Indexer
        /// <summary>
        /// Get value by given tagname(index) or
        /// create new tag by index and value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string? this[string index]
        {
            get
            {
                index = index.ToLowerInvariant();
                return _tags.ContainsKey(index) ? _tags[index] : null;
            }
            set
            {
                _tags[index.ToLowerInvariant()] = value;
            }
        }
        #endregion

        public BibEntry()
        {
        }

        public BibEntry(int sourcePosition)
        {
            SourcePosition = sourcePosition;
        }

        public Citation ToCitation()
        {
            var citation = new Citation();
            if (Title != null)
                citation[nameof(Title)] = new TextVariable(Title);
            if (Author != null)
                citation[nameof(Author)] = NamesVariable.Parse(Author, "and", ",");
            if (Volume != null && NumberVariable.TryParse(Volume, out var volumeVariable))
                citation[nameof(Volume)] = volumeVariable;
            if (Number != null && NumberVariable.TryParse(Number, out var numberVariable))
                citation[nameof(Number)] = numberVariable;
            if (Pages != null && NumberVariable.TryParse(Pages, out var pagesVariable))
                citation[nameof(Pages)] = pagesVariable;
            if (Abstract != null)
                citation[nameof(Abstract)] = new TextVariable(Abstract);

            return citation;
        }

        private string? Get([CallerMemberName] string? name = null)
        {
            return name == null ? null : this[name];
        }

        private void Set(string? value, [CallerMemberName] string? name = null)
        {
            if (name != null)
            {
                this[name] = value;
            }
        }
    }

    public class BibEntryItem
    {
        public int SourcePosition { get; set; }
        public int SourceLength { get; set; }
        public EntryItemType Type { get; set; }

        public BibEntryItem(int position, int length, EntryItemType type)
        {
            Type = type;
            SourcePosition = position;
            SourceLength = length;
        }
    }

    public enum EntryItemType
    {
        None,
        Type,
        Key,
        Name,
        Value
    }

    public enum EntryType
    {
        Article,
        Book,
        Booklet,
        Conference,
        InBook,
        InCollection,
        InProceedings,
        Manual,
        Mastersthesis,
        Misc,
        PhDThesis,
        Patent,
        Collection,
        Electronic,
        Proceedings,
        TechReport,
        Unpublished,
        Online
    }
}
