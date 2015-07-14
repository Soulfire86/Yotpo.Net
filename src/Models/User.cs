using System;
using Newtonsoft.Json;

namespace YotpoNet.Models
{
    public class NewUser
    {
        public int user_id { get; set; }
        public String token { get; set; }
        public String app_key { get; set; }
        public String secret { get; set; }
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

    public class UserToken
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
    }

    public class APICredentials
    {
        public String client_id { get; set; }
        public String client_secret { get; set; }
        public String grant_type { get; set; }

        public APICredentials()
        {
            grant_type = "client_credentials";
        }
    }

}
