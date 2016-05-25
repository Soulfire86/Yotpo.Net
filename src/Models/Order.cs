using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YotpoNet.Internal;

namespace YotpoNet.Models
{

    public class NewOrder
    {
        public string email { get; set; }
        public string customer_name { get; set; }
        public string user_id { get; set; }
        public string order_id { get; set; }
        public string currency_iso { get; set; }
        [JsonConverter(typeof(YotpoProductsJsonConverter))]
        public List<Product> products { get; set; }
        public bool validate_data { get; set; }
        public string order_date { get; set; } // In YYYY-MM-DD format
        public string utoken { get; set; }
        public string platform { get; set; }
    }

    public class Order
    {
        public int id { get; set; }
        public string user_email { get; set; }
        public string user_name { get; set; }
        public string product_sku { get; set; }
        public string order_id { get; set; }
        public string product_name { get; set; }
        public string product_url { get; set; }
        public DateTime order_date { get; set; }
        public string product_description { get; set; }
        public string product_image { get; set; }
        public DateTime delivery_date { get; set; }
        public DateTime created_at { get; set; }
    }

    public class OrdersToDelete
    {
        public string utoken { get; set; }
        public OrderToDelete[] orders { get; set; }
    }

    public class OrderToDelete
    {
        public string order_id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] skus { get; set; }
    }

}
