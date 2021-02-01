using sample_ecommerce_poco;
using sitemap_reader;
using System;
using System.Linq;

namespace sitemap_parse_console
{
    class Program
    {
        static void Main(string[] args)
        {
            var xml = SitemapParser.Parse<Urlset>(@"C:\Users\ahmed.guneysu\workspace\repos\sample-ecommerce\data\sitemap_244.xml");
            var data = xml.Url.Select(x => new Product()
            {
                Title = x.Image.Title,
                Image = x.Image.Loc,
                Link = x.Loc
            });

            //var json = JsonConvert.SerializeObject(xml);
            Console.WriteLine("Hello World!");
        }
    }
}
