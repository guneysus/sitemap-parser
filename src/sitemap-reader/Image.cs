using System.Xml.Serialization;
using System.Xml;

namespace sitemap_reader
{
    [XmlRoot(ElementName = "image", Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
    public class Image
    {
        [XmlElement(ElementName = "loc", Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
        public string Loc { get; set; }
        [XmlElement(ElementName = "title", Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
        public string Title { get; set; }
    }

}
