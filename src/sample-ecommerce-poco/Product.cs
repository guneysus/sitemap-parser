using System;

namespace sample_ecommerce_poco
{
    public class Product
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
    }
}
