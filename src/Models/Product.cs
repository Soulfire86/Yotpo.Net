using System;

namespace YotpoNet.Models
{
    public class Product
    {
        public String ProductSKU { get; set; }
        public ProductData ProductData { get; set; }
    }

    public class ProductData
    {
        public String name { get; set; }
        public String url { get; set; }
        public String image { get; set; }
        public String description { get; set; }
        public double price { get; set; }
        public String product_tags { get; set; }
    }
}
