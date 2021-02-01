using System.Xml.Serialization;
using System.Xml;

namespace sitemap_reader
{
    [XmlRoot(ElementName = "link", Namespace = "http://www.w3.org/1999/xhtml")]
    public class Link
    {
        [XmlAttribute(AttributeName = "rel")]
        public string Rel { get; set; }

        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }

}
