using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling
{
    /// <summary>
    /// Base class for all xml elements.
    /// </summary>
    public abstract class Element
    {
        ///// <summary>
        ///// Constructor.
        ///// </summary>
        //protected Element()
        //{
        //    // init
        //    if (File.Current != null)
        //    {
        //        this.Tag = string.Format("({0}, {1})", File.Current.LineNumber, File.Current.LinePosition);
        //    }
        //}

        

        /// <summary>
        /// Gets or sets additional data for the element. When the element was loaded using an XmlReader,
        /// the tag is filled with the (line, position) of the element in the xml.
        /// </summary>
        [XmlIgnore]
        public string? Tag
        {
            get;
            private set;
        }
    }
}
