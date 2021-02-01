using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;

namespace sitemap_reader
{

    [XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Urlset
    {
        [XmlElement(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public List<Url> Url { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        [JsonIgnore]
        public string Xmlns { get; set; }

        [JsonIgnore]
        [XmlAttribute(AttributeName = "xhtml", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xhtml { get; set; }

        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        [JsonIgnore]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        [JsonIgnore]
        public string SchemaLocation { get; set; }

        [JsonIgnore]
        [XmlAttribute(AttributeName = "image", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Image { get; set; }

    }
}
