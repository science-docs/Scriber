using Scriber.Util;
using System.Runtime.CompilerServices;

namespace Scriber.Bibliography
{
    public class CitationBuilder
    {
        private readonly Citation citation;

        public CitationBuilder(string key, string type)
        {
            citation = new Citation(key);
            citation[nameof(type)] = new TextVariable(type);
        }

        public CitationBuilder(string key, CitationType type) : this(key, TransformType(type))
        {
        }

        private static string TransformType(CitationType type)
        {
            return type switch
            {
                CitationType.ArticleMagazine => "article-magazine",
                CitationType.ArticleNewspaper => "article-newspaper",
                CitationType.ArticleJournal => "article-journal",
                CitationType.EntryDictionary => "entry-dictionary",
                CitationType.EntryEncyclopedia => "entry-encyclopedia",
                CitationType.LegalCase => "legal_case",
                CitationType.MotionPicture => "motion_picture",
                CitationType.MusicalScore => "musical_score",
                CitationType.PaperConference => "paper-conference",
                CitationType.PostWeblog => "post-webblog",
                CitationType.PersonalCommunication => "personal_communication",
                CitationType.ReviewBook => "review-book",
                _ => EnumUtility.GetName(type)?.ToLowerInvariant() ?? string.Empty
            };
        }

        #region Standard Variables

        public void Abstract(ITextVariable @abstract)
        {
            Set(@abstract);
        }

        public void Annote(ITextVariable annote)
        {
            Set(annote);
        }

        public void Archive(ITextVariable archive)
        {
            Set(archive);
        }

        public void ArchiveLocation(ITextVariable archiveLocation)
        {
            Set(archiveLocation);
        }

        public void ArchivePlace(ITextVariable archivePlace)
        {
            Set(archivePlace);
        }

        public void Authority(ITextVariable authority)
        {
            Set(authority);
        }

        public void CallNumber(ITextVariable callNumber)
        {
            Set(callNumber);
        }

        public void CitationLabel(ITextVariable citationLabel)
        {
            Set(citationLabel);
        }

        public void CitationNumber(ITextVariable citationNumber)
        {
            Set(citationNumber);
        }

        public void ContainerTitle(ITextVariable containerTitle)
        {
            Set(containerTitle);
        }

        public void CollectionTitle(ITextVariable collectionTitle)
        {
            Set(collectionTitle);
        }

        public void CollectionTitleShort(ITextVariable collectionTitleShort)
        {
            Set(collectionTitleShort);
        }

        public void Dimensions(ITextVariable dimensions)
        {
            Set(dimensions);
        }

        public void Doi(ITextVariable doi)
        {
            Set(doi);
        }

        public void Event(ITextVariable @event)
        {
            Set(@event);
        }

        public void EventPlace(ITextVariable eventPlace)
        {
            Set(eventPlace);
        }

        public void FirstReferenceNoteNumber(ITextVariable firstReferenceNoteNumber)
        {
            Set(firstReferenceNoteNumber);
        }

        public void Genre(ITextVariable genre)
        {
            Set(genre);
        }

        public void Isbn(ITextVariable isbn)
        {
            Set(isbn);
        }

        public void Issn(ITextVariable issn)
        {
            Set(issn);
        }

        public void Jurisdiction(ITextVariable jurisdiction)
        {
            Set(jurisdiction);
        }

        public void Keyword(ITextVariable keyword)
        {
            Set(keyword);
        }

        public void Locator(ITextVariable locator)
        {
            Set(locator);
        }

        public void Medium(ITextVariable medium)
        {
            Set(medium);
        }

        public void Note(ITextVariable note)
        {
            Set(note);
        }

        public void OriginalPublisher(ITextVariable originalPublisher)
        {
            Set(originalPublisher);
        }

        public void OriginalPublisherPlace(ITextVariable originalPublisherPlace)
        {
            Set(originalPublisherPlace);
        }

        public void OriginalTitle(ITextVariable originalTitle)
        {
            Set(originalTitle);
        }

        public void Page(ITextVariable page)
        {
            Set(page);
        }

        public void Page(INumberVariable page)
        {
            Set(page);
        }

        public void PageFirst(ITextVariable pageFirst)
        {
            Set(pageFirst);
        }

        public void PageFirst(INumberVariable pageFirst)
        {
            Set(pageFirst);
        }

        public void Pmcid(ITextVariable pmcid)
        {
            Set(pmcid);
        }

        public void Pmid(ITextVariable pmid)
        {
            Set(pmid);
        }

        public void Publisher(ITextVariable publisher)
        {
            Set(publisher);
        }

        public void PublisherPlace(ITextVariable publisherPlace)
        {
            Set(publisherPlace);
        }

        public void References(ITextVariable references)
        {
            Set(references);
        }

        public void ReviewedTitle(ITextVariable reviewedTitle)
        {
            Set(reviewedTitle);
        }

        public void Scale(ITextVariable scale)
        {
            Set(scale);
        }

        public void Section(ITextVariable section)
        {
            Set(section);
        }

        public void Source(ITextVariable source)
        {
            Set(source);
        }

        public void Status(ITextVariable status)
        {
            Set(status);
        }

        public void Title(ITextVariable title)
        {
            Set(title);
        }

        public void TitleShort(ITextVariable titleShort)
        {
            Set(titleShort);
        }

        public void Url(ITextVariable url)
        {
            Set(url);
        }

        public void Version(ITextVariable version)
        {
            Set(version);
        }

        public void YearSuffix(ITextVariable yearSuffix)
        {
            Set(yearSuffix);
        }

        #endregion

        #region Number Variables

        public void ChapterNumber(INumberVariable chapterNumber)
        {
            Set(chapterNumber);
        }

        public void CollectionNumber(INumberVariable collectionNumber)
        {
            Set(collectionNumber);
        }

        public void Edition(INumberVariable edition)
        {
            Set(edition);
        }

        public void Issue(INumberVariable issue)
        {
            Set(issue);
        }

        public void Number(INumberVariable number)
        {
            Set(number);
        }

        public void NumberOfPages(INumberVariable numberOfPages)
        {
            Set(numberOfPages);
        }

        public void NumberOfVolumes(INumberVariable numberOfVolumes)
        {
            Set(numberOfVolumes);
        }

        public void Volume(INumberVariable volume)
        {
            Set(volume);
        }

        #endregion

        #region Date Variables

        public void Accessed(IDateVariable accessed)
        {
            Set(accessed);
        }

        public void Container(IDateVariable container)
        {
            Set(container);
        }

        public void EventDate(IDateVariable eventDate)
        {
            Set(eventDate);
        }

        public void Issued(IDateVariable issued)
        {
            Set(issued);
        }

        public void OriginalDate(IDateVariable originalDate)
        {
            Set(originalDate);
        }

        public void Submitted(IDateVariable submitted)
        {
            Set(submitted);
        }

        #endregion

        #region Name Variables

        public void Author(INamesVariable author)
        {
            Set(author);
        }

        public void CollectionAuthor(INamesVariable collectionAuthor)
        {
            Set(collectionAuthor);
        }

        public void Composer(INamesVariable composer)
        {
            Set(composer);
        }

        public void ContainerAuthor(INamesVariable containerAuthor)
        {
            Set(containerAuthor);
        }

        public void Director(INamesVariable directory)
        {
            Set(directory);
        }

        public void Editor(INamesVariable editor)
        {
            Set(editor);
        }

        public void EditorialDirector(INamesVariable editorialDirector)
        {
            Set(editorialDirector);
        }

        public void Illustrator(INamesVariable illustrator)
        {
            Set(illustrator);
        }

        public void Interviewer(INamesVariable interviewer)
        {
            Set(interviewer);
        }

        public void OriginalAuthor(INamesVariable originalAuthor)
        {
            Set(originalAuthor);
        }

        public void Recipient(INamesVariable recipient)
        {
            Set(recipient);
        }

        public void ReviewedAuthor(INamesVariable reviewedAuthor)
        {
            Set(reviewedAuthor);
        }

        public void Translator(INamesVariable translator)
        {
            Set(translator);
        }

        #endregion

        internal void Set(IVariable variable, [CallerMemberName] string? name = null)
        {
            if (name != null)
            {
                citation[name.ToKebapCase()] = variable;
            }
        }

        public Citation Build()
        {
            return citation;
        }
    }
}
