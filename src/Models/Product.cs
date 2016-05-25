using System;

namespace YotpoNet.Models
{
    public class Product
    {
        public string ProductSKU { get; set; }
        public ProductData ProductData { get; set; }
    }

    public class ProductData
    {
        public string name { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string product_tags { get; set; }
    }
}
