using System.Xml.Serialization;
using System.Xml;

namespace sitemap_reader
{
    public class SitemapParser
    {
        public static T Parse<T>(string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Urlset));
            T xml;
            using (XmlReader reader = XmlReader.Create(path))
            {
                xml = (T)ser.Deserialize(reader);
            }

            return xml;
        }
    }

}
