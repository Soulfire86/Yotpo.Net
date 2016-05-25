using System;
using Newtonsoft.Json;

namespace YotpoNet.Models
{
    public class NewUser
    {
        public int user_id { get; set; }
        public string token { get; set; }
        public string app_key { get; set; }
        public string secret { get; set; }
    }

    public class UserProfile
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public int Score { get; set; }
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        [JsonProperty(PropertyName = "social_image")]
        public string SocialImage { get; set; }
    }

    public class ReviewUser
    {
        public int user_id { get; set; }
        public string display_name { get; set; }
        public string socialimage { get; set; }
        public string user_type { get; set; }
        public bool is_social_connected { get; set; }
    }

    public class UserToken
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
    }

    public class APICredentials
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }

        public APICredentials()
        {
            grant_type = "client_credentials";
        }
    }

}
