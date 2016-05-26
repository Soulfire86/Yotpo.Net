using System;
using System.Net;
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
            var client = new RestClient(BaseURL) {Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(_clientId, _clientSecret)};
            var response = client.Execute<T>(request);

            if (response.ErrorException == null)
                return response.Data;

            const string message = "Error retrieving response. Check inner details for more info.";
            var yotpoException = new ApplicationException(message, response.ErrorException);
            throw yotpoException;
        }

        public new IRestResponse Execute(IRestRequest request)
        {
            var client = new RestClient(BaseURL) {Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(_clientId, _clientSecret)};
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

        public int GetOrders(string utoken, string page = "1", string count = "200", string since_id = null)
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
        public int SendTestEmail(string utoken, string email = null)
        {
            var request = new RestRequest("apps/{app_key}/reminders/send_test_email", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("app_key", _clientId);

            var json = "{\"utoken\": \"" + utoken + "\"";
            if (!string.IsNullOrEmpty(email))
                json += ", \"email\": \"" + email + "\"";
            json += "}";

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = Execute(request) as RestResponse;

            if (response != null)
            {
                JToken content = JObject.Parse(response.Content);

                var statusCode = (int)content.SelectToken("status.code");

                return statusCode;
            }

            return 0;
        }


        // REVIEWS
        #region Reviews
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public BottomLine GetBottomLine(string productID)
        {
            var bottomLine = new BottomLine {average_score = 0, total_reviews = 0};
            var request = new RestRequest("products/{app_key}/{product_id}/bottomline", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("app_key", _clientId);
            request.AddUrlSegment("product_id", productID);

            var response = Execute(request) as RestResponse;

            if (response != null)
            {
                JToken content = JObject.Parse(response.Content);

                var statusCode = (int) content.SelectToken("status.code");
                if (statusCode == 200)
                    bottomLine = content["response"]["bottomline"].ToObject<BottomLine>();
            }

            return bottomLine;
        }

        public ProductReviews GetReviewsForProduct(string productID, int pageNum=1, int resultsPerPage=5)
        {
            var request = new RestRequest("v1/widget/{app_key}/products/{product_id}/reviews.json", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("app_key", _clientId);
            request.AddUrlSegment("product_id", productID);
            request.AddQueryParameter("page", pageNum.ToString());
            request.AddQueryParameter("per_page", resultsPerPage.ToString());

            var response = Execute(request) as RestResponse;

            if (response != null)
            {
                JToken content = JObject.Parse(response.Content);
                var statusCode = (int)content.SelectToken("status.code");
                if (statusCode == 200)
                {
                    var prodReviews = content["response"].ToObject<ProductReviews>();

                    return prodReviews;
                }
            }

            return new ProductReviews();
        }

        #endregion
    }
}