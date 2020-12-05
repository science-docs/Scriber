using Scriber.Layout.Document;
using System;
using System.Collections.Generic;
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
            get => Get();
            set => Set(value);
        }


        public string? Addendum
        {
            get => Get();
            set => Set(value);
        }


        public string? Afterword
        {
            get => Get();
            set => Set(value);
        }


        public string? Annotation
        {
            get => Get();
            set => Set(value);
        }


        public string? Annotator
        {
            get => Get();
            set => Set(value);
        }


        public string? Author
        {
            get => Get();
            set => Set(value);
        }


        public string? BookAuthor
        {
            get => Get();
            set => Set(value);
        }


        public string? BookTitle
        {
            get => Get();
            set => Set(value);
        }


        public string? Commentator
        {
            get => Get();
            set => Set(value);
        }


        public string? Date
        {
            get => Get();
            set => Set(value);
        }


        public string? Doi
        {
            get => Get();
            set => Set(value);
        }


        public string? Edition
        {
            get => Get();
            set => Set(value);
        }


        public string? Editor
        {
            get => Get();
            set => Set(value);
        }


        public string? Eid
        {
            get => Get();
            set => Set(value);
        }


        public string? EntrySubType
        {
            get => Get();
            set => Set(value);
        }


        public string? EPrint
        {
            get => Get();
            set => Set(value);
        }


        public string? EPrintType
        {
            get => Get();
            set => Set(value);
        }


        public string? EventDate
        {
            get => Get();
            set => Set(value);
        }


        public string? EventTitle
        {
            get => Get();
            set => Set(value);
        }


        public string? File
        {
            get => Get();
            set => Set(value);
        }


        public string? Forward
        {
            get => Get();
            set => Set(value);
        }


        public string? Holder
        {
            get => Get();
            set => Set(value);
        }


        public string? HowPublished
        {
            get => Get();
            set => Set(value);
        }


        public string? IndexTitle
        {
            get => Get();
            set => Set(value);
        }


        public string? Institution
        {
            get => Get();
            set => Set(value);
        }


        public string? Introduction
        {
            get => Get();
            set => Set(value);
        }


        public string? Isan
        {
            get => Get();
            set => Set(value);
        }


        public string? Isbn
        {
            get => Get();
            set => Set(value);
        }


        public string? Ismn
        {
            get => Get();
            set => Set(value);
        }


        public string? Isrn
        {
            get => Get();
            set => Set(value);
        }


        public string? Issn
        {
            get => Get();
            set => Set(value);
        }


        public string? Issue
        {
            get => Get();
            set => Set(value);
        }


        public string? IssueTitle
        {
            get => Get();
            set => Set(value);
        }


        public string? Iswc
        {
            get => Get();
            set => Set(value);
        }


        public string? JournalTitle
        {
            get => Get();
            set => Set(value);
        }


        public string? Label
        {
            get => Get();
            set => Set(value);
        }


        public string? Language
        {
            get => Get();
            set => Set(value);
        }


        public string? Library
        {
            get => Get();
            set => Set(value);
        }


        public string? Location
        {
            get => Get();
            set => Set(value);
        }


        public string? Maintitle
        {
            get => Get();
            set => Set(value);
        }


        public string? Note
        {
            get => Get();
            set => Set(value);
        }


        public string? Number
        {
            get => Get();
            set => Set(value);
        }


        public string? Organization
        {
            get => Get();
            set => Set(value);
        }


        public string? Pages
        {
            get => Get();
            set => Set(value);
        }


        public string? PageTotal
        {
            get => Get();
            set => Set(value);
        }


        public string? Pagination
        {
            get => Get();
            set => Set(value);
        }


        public string? Part
        {
            get => Get();
            set => Set(value);
        }


        public string? Publisher
        {
            get => Get();
            set => Set(value);
        }


        public string? PubState
        {
            get => Get();
            set => Set(value);
        }


        public string? ReprintTitle
        {
            get => Get();
            set => Set(value);
        }


        public string? Series
        {
            get => Get();
            set => Set(value);
        }


        public string? Shorthand
        {
            get => Get();
            set => Set(value);
        }


        public string? ShorthandIntro
        {
            get => Get();
            set => Set(value);
        }


        public string? Title
        {
            get => Get();
            set => Set(value);
        }


        public string? Translator
        {
            get => Get();
            set => Set(value);
        }


        public string? Url
        {
            get => Get();
            set => Set(value);
        }


        public string? UrlDate
        {
            get => Get();
            set => Set(value);
        }


        public string? Venue
        {
            get => Get();
            set => Set(value);
        }


        public string? Version
        {
            get => Get();
            set => Set(value);
        }


        public string? Volume
        {
            get => Get();
            set => Set(value);
        }


        public string? Volumes
        {
            get => Get();
            set => Set(value);
        }

        // BibText entries

        public string? Address
        {
            get => Get();
            set => Set(value);
        }

        public string? Annote
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

        public string? Howpublished
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Journal
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? Month
        {
            get { return Get(); }
            set { Set(value); }
        }

        public string? School
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
        public string Type
        {
            get
            {
                return Enum.GetName(typeof(EntryType), _type) ?? "Misc";
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
                _tags.TryGetValue(index, out var value);
                return value;
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
            var citationType = GetCitationType();
            var cb = new CitationBuilder(Key, citationType);

            if (!string.IsNullOrWhiteSpace(Abstract))
                cb.Abstract(new TextVariable(Abstract));

            if (!string.IsNullOrWhiteSpace(Annote))
                cb.Annote(new TextVariable(Annote));

            if (!string.IsNullOrWhiteSpace(Location))
            {
                cb.EventPlace(new TextVariable(Location));
                cb.PublisherPlace(new TextVariable(Location));
            }
            else if (!string.IsNullOrWhiteSpace(Address))
            {
                cb.EventPlace(new TextVariable(Address));
                cb.PublisherPlace(new TextVariable(Address));
            }
            if (!string.IsNullOrWhiteSpace(Venue))
            {
                cb.EventPlace(new TextVariable(Venue));
            }

            if (!string.IsNullOrWhiteSpace(Author))
                cb.Author(ParseName(Author));
            else if (!string.IsNullOrWhiteSpace(BookAuthor))
                cb.Author(ParseName(BookAuthor));

            if (!string.IsNullOrWhiteSpace(Editor))
                cb.Editor(ParseName(Editor));

            if (UrlDate != null && DateVariable.TryParse(UrlDate, out var urlDateVariable))
            {
                cb.Accessed(urlDateVariable);
            }
            if (!string.IsNullOrWhiteSpace(Url))
            {
                cb.Url(new TextVariable(Url));
            }

            if (Date != null && DateVariable.TryParse(Date, out var dateVariable))
            {
                cb.Issued(dateVariable);
                cb.EventDate(dateVariable);
            }
            else if (Year != null && DateVariable.TryParse(Year, Month, out var yearMonthVariable))
            {
                cb.Issued(yearMonthVariable);
                cb.EventDate(yearMonthVariable);
            }

            ITextVariable? containerTitle = null;
            if (!string.IsNullOrWhiteSpace(Journal))
            {
                containerTitle = new TextVariable(Journal);
            }
            else if (!string.IsNullOrWhiteSpace(JournalTitle))
            {
                containerTitle = new TextVariable(JournalTitle);
            }
            else if (!string.IsNullOrWhiteSpace(BookTitle))
            {
                containerTitle = new TextVariable(BookTitle);
            }
            else if (!string.IsNullOrWhiteSpace(Series))
            {
                containerTitle = new TextVariable(Series);
                
            }

            if (containerTitle != null)
            {
                cb.ContainerTitle(containerTitle);
                cb.CollectionTitle(containerTitle);
            }


            if (Number != null && NumberVariable.TryParse(Number, out var numberVariable))
            {
                cb.Number(numberVariable);
            }

            if (Issue != null && NumberVariable.TryParse(Issue, out var issueVariable))
            {
                cb.Issue(issueVariable);
            }
            else if (Number != null && NumberVariable.TryParse(Number, out var numberIssueVariable))
            {
                cb.Issue(numberIssueVariable);
            }
            
            if (!string.IsNullOrWhiteSpace(Publisher))
            {
                cb.Publisher(new TextVariable(Publisher));
            }
            else if (!string.IsNullOrWhiteSpace(Organization))
            {
                cb.Publisher(new TextVariable(Organization));
            }
            else if (!string.IsNullOrWhiteSpace(Institution))
            {
                cb.Publisher(new TextVariable(Institution));
            }
            else if (!string.IsNullOrWhiteSpace(School))
            {
                cb.Publisher(new TextVariable(School));
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                cb.Title(new TextVariable(Title));
            }

            if (!string.IsNullOrWhiteSpace(Version))
                cb.Version(new TextVariable(Version));

            if (!string.IsNullOrWhiteSpace(Doi))
                cb.Doi(new TextVariable(Doi));

            if (!string.IsNullOrWhiteSpace(Isbn))
                cb.Isbn(new TextVariable(Isbn));

            if (!string.IsNullOrWhiteSpace(Issn))
                cb.Issn(new TextVariable(Issn));

            if (!string.IsNullOrWhiteSpace(Translator))
                cb.Translator(ParseName(Translator));

            if (Chapter != null && NumberVariable.TryParse(Chapter, out var chapterVariable))
                cb.ChapterNumber(chapterVariable);

            if (Edition != null && NumberVariable.TryParseOrdinal(Edition, out var editionVariable))
                cb.Edition(editionVariable);

            if (Volume != null && NumberVariable.TryParse(Volume, out var volumeVariable))
                cb.Volume(volumeVariable);

            if (Volumes != null && NumberVariable.TryParse(Volumes, out var volumesVariable))
                cb.NumberOfVolumes(volumesVariable);

            bool hasPageTotal = false;
            if (PageTotal != null && NumberVariable.TryParse(PageTotal, out var pageTotalVariable))
            {
                hasPageTotal = true;
                cb.NumberOfPages(pageTotalVariable);
            }

            if (Pages != null && NumberVariable.TryParse(Pages, out var pagesVariable))
            {
                cb.Page(pagesVariable);
                if (pagesVariable.Max != pagesVariable.Min)
                {
                    cb.PageFirst(new NumberVariable(pagesVariable.Min));

                    if (!hasPageTotal)
                    {
                        cb.NumberOfPages(new NumberVariable(pagesVariable.Max - pagesVariable.Min));
                    }
                }
            }
                

            return cb.Build();
        }

        private NamesVariable ParseName(string name)
        {
            return NamesVariable.Parse(name, "and", ",");
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

        private CitationType GetCitationType()
        {
            return _type switch
            {
                EntryType.Article => CitationType.ArticleJournal,
                EntryType.Proceedings => CitationType.Book,
                EntryType.Manual => CitationType.Book,
                EntryType.Book => CitationType.Book,
                EntryType.Periodical => CitationType.Book,
                EntryType.Booklet => CitationType.Pamphlet,
                EntryType.InBook => CitationType.Chapter,
                EntryType.InCollection => CitationType.Chapter,
                EntryType.InProceedings => CitationType.PaperConference,
                EntryType.Conference => CitationType.PaperConference,
                EntryType.Mastersthesis => CitationType.Thesis,
                EntryType.PhDThesis => CitationType.Thesis,
                EntryType.Thesis => CitationType.Thesis,
                EntryType.TechReport => CitationType.Report,
                EntryType.Patent => CitationType.Patent,
                EntryType.Electronic => CitationType.Webpage,
                EntryType.Online => CitationType.Webpage,
                EntryType.Unpublished => CitationType.Manuscript,
                _ => CitationType.Article
            };
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
        // BibLaTex types
        Article, 
        Book, 
        BookInBook, 
        Booklet, 
        Collection, 
        InBook, 
        InCollection,
        InProceedings, 
        InReference, 
        Manual, 
        Misc, 
        MVBook, 
        MVCollection, 
        MVProceedings, 
        MVReference, 
        Online, 
        Patent, 
        Periodical, 
        Proceedings, 
        Reference, 
        Report, 
        Set, 
        SuppBook, 
        SuppCollection, 
        SuppPeriodical, 
        Thesis, 
        Unpublished, 
        XData,

        // Leftover BibTex types
        Conference,
        Mastersthesis,
        PhDThesis,
        Electronic,
        TechReport,
    }
}
