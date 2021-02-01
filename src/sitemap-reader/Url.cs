using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace sitemap_reader
{
    [XmlRoot(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Url
    {
        [XmlElement(ElementName = "loc", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public string Loc { get; set; }

        [XmlElement(ElementName = "priority", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        [JsonIgnore]
        public string Priority { get; set; }

        [XmlElement(ElementName = "link", Namespace = "http://www.w3.org/1999/xhtml")]
        [JsonIgnore]
        public Link Link { get; set; }

        [XmlElement(ElementName = "image", Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
        public Image Image { get; set; }
    }

}
