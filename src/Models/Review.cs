using System;

namespace YotpoNet.Models
{
    public class ProductReviews
    {
        public Pagination pagination { get; set; }
        public BottomLine bottomline { get; set; }
        public object[] products { get; set; } //Why is this even returned?
        public object[] product_tags { get; set; }
        public Review[] reviews { get; set; }
    }

    public class Review
    {
        public int id { get; set; }
        public int score { get; set; }
        public int votes_up { get; set; }
        public int votes_down { get; set; }
        public string content { get; set; }
        public string title { get; set; }
        public DateTime create_at { get; set; }
        public bool verified_buyer { get; set; }
        public int? source_review_id { get; set; }
        public int product_id { get; set; }
        public ReviewUser user { get; set; }
    }

    public class Pagination
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
    }

    public class BottomLine
    {
        public string domain_key { get; set; }
        public int total_reviews { get; set; }
        public double average_score { get; set; }
    }

}
