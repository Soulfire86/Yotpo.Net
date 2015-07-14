using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YotpoNet.Internal;

namespace YotpoNet.Models
{

    public class NewOrder
    {
        public String email { get; set; }
        public String customer_name { get; set; }
        public String user_id { get; set; }
        public String order_id { get; set; }
        public String currency_iso { get; set; }
        [JsonConverter(typeof(YotpoProductsJsonConverter))]
        public List<Product> products { get; set; }
        public bool validate_data { get; set; }
        public String order_date { get; set; } // In YYYY-MM-DD format
        public String utoken { get; set; }
        public String platform { get; set; }
    }

    public class Order
    {
        public int id { get; set; }
        public String user_email { get; set; }
        public String user_name { get; set; }
        public String product_sku { get; set; }
        public String order_id { get; set; }
        public String product_name { get; set; }
        public String product_url { get; set; }
        public DateTime order_date { get; set; }
        public String product_description { get; set; }
        public String product_image { get; set; }
        public DateTime delivery_date { get; set; }
        public DateTime created_at { get; set; }
    }

}
