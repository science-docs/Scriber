using Scriber.Util;
using System.Runtime.CompilerServices;

namespace Scriber.Bibliography
{
    public class CitationBuilder
    {
        private readonly Citation citation;

        public CitationBuilder(string key, CitationType type)
        {
            citation = new Citation(key);
            citation[nameof(type)] = new TextVariable(TransformType(type));
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
            Set(archiveLocation, "archive_location");
        }

        public void ArchivePlace(ITextVariable archivePlace)
        {
            Set(archivePlace, "archive-place");
        }

        public void Authority(ITextVariable authority)
        {
            Set(authority);
        }

        public void CallNumber(ITextVariable callNumber)
        {
            Set(callNumber, "call-number");
        }

        public void CitationLabel(ITextVariable citationLabel)
        {
            Set(citationLabel, "citation-label");
        }

        public void CitationNumber(ITextVariable citationNumber)
        {
            Set(citationNumber, "citation-number");
        }

        public void ContainerTitle(ITextVariable containerTitle)
        {
            Set(containerTitle, "container-title");
        }

        public void CollectionTitle(ITextVariable collectionTitle)
        {
            Set(collectionTitle, "collection-title");
        }

        public void CollectionTitleShort(ITextVariable collectionTitleShort)
        {
            Set(collectionTitleShort, "collection-title-short");
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
            Set(eventPlace, "event-place");
        }

        public void FirstReferenceNoteNumber(ITextVariable firstReferenceNoteNumber)
        {
            Set(firstReferenceNoteNumber, "first-reference-note-number");
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
            Set(originalPublisher, "original-publisher");
        }

        public void OriginalPublisherPlace(ITextVariable originalPublisherPlace)
        {
            Set(originalPublisherPlace, "original-publisher-place");
        }

        public void OriginalTitle(ITextVariable originalTitle)
        {
            Set(originalTitle, "original-title");
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
            Set(pageFirst, "page-first");
        }

        public void PageFirst(INumberVariable pageFirst)
        {
            Set(pageFirst, "page-first");
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
            Set(publisherPlace, "publisher-place");
        }

        public void References(ITextVariable references)
        {
            Set(references);
        }

        public void ReviewedTitle(ITextVariable reviewedTitle)
        {
            Set(reviewedTitle, "reviewed-title");
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
            Set(titleShort, "title-short");
        }

        public void Url(ITextVariable url)
        {
            Set(url, "original-title");
        }

        public void Version(ITextVariable version)
        {
            Set(version);
        }

        public void YearSuffix(ITextVariable yearSuffix)
        {
            Set(yearSuffix, "year-suffix");
        }

        #endregion

        #region Number Variables

        public void ChapterNumber(INumberVariable chapterNumber)
        {
            Set(chapterNumber, "chapter-number");
        }

        public void CollectionNumber(INumberVariable collectionNumber)
        {
            Set(collectionNumber, "collection-number");
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
            Set(numberOfPages, "number-of-pages");
        }

        public void NumberOfVolumes(INumberVariable numberOfVolumes)
        {
            Set(numberOfVolumes, "number-of-volumes");
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
            Set(eventDate, "event-date");
        }

        public void Issued(IDateVariable issued)
        {
            Set(issued);
        }

        public void OriginalDate(IDateVariable originalDate)
        {
            Set(originalDate, "original-date");
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
            Set(collectionAuthor, "collection-author");
        }

        public void Composer(INamesVariable composer)
        {
            Set(composer);
        }

        public void ContainerAuthor(INamesVariable containerAuthor)
        {
            Set(containerAuthor, "container-author");
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
            Set(editorialDirector, "editorial-director");
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
            Set(originalAuthor, "original-author");
        }

        public void Recipient(INamesVariable recipient)
        {
            Set(recipient);
        }

        public void ReviewedAuthor(INamesVariable reviewedAuthor)
        {
            Set(reviewedAuthor, "reviewed-author");
        }

        public void Translator(INamesVariable translator)
        {
            Set(translator);
        }

        #endregion

        private void Set(IVariable variable, [CallerMemberName] string? name = null)
        {
            if (name != null)
            {
                citation[name] = variable;
            }
        }

        public Citation Build()
        {
            return citation;
        }
    }
}
