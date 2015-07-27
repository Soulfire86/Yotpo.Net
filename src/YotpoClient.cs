using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using YotpoNet.Models;

namespace YotpoNet
{
    // YotpoApi.cs
    public class YotpoClient : RestClient
    {
        private const string BaseURL = "https://api.yotpo.com/";

        private readonly string _clientId;
        private readonly string _clientSecret;

        public YotpoClient(string clientId, string clientSecret)
                : base(BaseURL)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(BaseURL) {Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret)};
            var response = client.Execute<T>(request);

            if (response.ErrorException == null)
                return response.Data;

            const string message = "Error retrieving response. Check inner details for more info.";
            var yotpoException = new ApplicationException(message, response.ErrorException);
            throw yotpoException;
        }

        public IRestResponse Execute(IRestRequest request)
        {
            var client = new RestClient(BaseURL) {Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret)};
            var response = client.Execute(request);
            if(response.ErrorException == null)
                return response;

            const string message = "Error retrieving response. Check inner details for more info.";
            var yotpoException = new ApplicationException(message, response.ErrorException);
            throw yotpoException;
        }


        // AUTHENTICATION

        public UserToken RequestOAuthToken()
        {
            var request = new RestRequest("oauth/token", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddBody(new APICredentials { client_id = _clientId, client_secret = _clientSecret});

            return Execute<UserToken>(request);
        }

        // USERS

        public NewUser CreateNewUser()
        {
            var request = new RestRequest("users", Method.POST) { RequestFormat = DataFormat.Json };

            return Execute<NewUser>(request);
        }

        public UserProfile GetUserProfile(int userId)
        {
            var request = new RestRequest("users/{user_id}", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("user_id", userId.ToString());

            var resp = Execute(request) as RestResponse;

            if (resp != null)
            {
                JToken responseContent = JObject.Parse(resp.Content);

                var statusCode = (int) responseContent.SelectToken("status.code");

                var userProfile = responseContent["response"]["user"].ToObject<UserProfile>();

                if (statusCode == 200)
                    return userProfile;
            }

            return new UserProfile();
        }

        // ORDERS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int CreateOrder(NewOrder order)
        {
            var request = new RestRequest("apps/{app_key}/purchases", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("app_key", _clientId);

            var serializedOrder = JsonConvert.SerializeObject(order);

            request.AddParameter("application/json", serializedOrder, ParameterType.RequestBody);

            var resp = Execute(request) as RestResponse;

            if (resp != null)
            {
                JToken responseContent = JObject.Parse(resp.Content);

                var statusCode = (int) responseContent.SelectToken("code");

                return statusCode;
            }

            return 0;
        }

        public int DeleteOrders(OrdersToDelete ordersToDelete)
        {
            var request = new RestRequest("apps/{app_key}/purchases", Method.DELETE) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("app_key", _clientId);

            var serializedOrders = JsonConvert.SerializeObject(ordersToDelete);

            request.AddParameter("application/json", serializedOrders, ParameterType.RequestBody);

            var response = Execute(request) as RestResponse;

            if (response != null)
            {
                var statusCode = (int) JObject.Parse(response.Content).SelectToken("code");

                return statusCode;
            }

            return 0;
        }

        public int GetOrders(String utoken, String page = "1", String count = "10", String since_id = null)
        {
            var request = new RestRequest("apps/{app_key}/purchases", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("app_key", _clientId);
            request.AddQueryParameter("utoken", utoken);

            var response = Execute(request) as RestResponse;

            if (response != null)
            {
                JToken content = JObject.Parse(response.Content);

                var statusCode = (int) content.SelectToken("status.code");

                return statusCode;
            }

            return 0;

            //TODO: Actually finish implementing this, I already know it works, just need to deserialize to C#
            //TODO: instead of just returning status code for sake of Tests.
        }

        /// <summary>
        /// Test Mail After Purchase
        /// <para>This requests sends the email template with all the placeholders and not real data, just the things that will be common in all emails that will be sent.</para>
        /// </summary>
        /// <param name="utoken">Valid Yotpo User Access Token</param>
        /// <param name="email">Optional email address</param>
        /// <returns>Returns response status code.</returns>
        public int SendTestEmail(String utoken, String email = null)
        {
            var request = new RestRequest("apps/{app_key}/reminders/send_test_email", Method.POST) { RequestFormat = DataFormat.Json};
            request.AddUrlSegment("app_key", _clientId);

            var json = "{\"utoken\": \"" + utoken + "\"";
            if (!String.IsNullOrEmpty(email))
                json += ", \"email\": \"" + email + "\"";
            json += "}";

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = Execute(request) as RestResponse;

            if (response != null)
            {
                JToken content = JObject.Parse(response.Content);

                return (int) content.SelectToken("code");
            }

            return 0;
        }

    }
}