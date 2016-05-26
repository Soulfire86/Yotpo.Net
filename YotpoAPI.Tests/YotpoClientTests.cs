using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using YotpoNet;
using YotpoNet.Models;

namespace YotpoAPI.Tests
{
    [TestClass]
    public class YotpoClientTests
    {

        private readonly YotpoClient _client;
        private readonly string _email;

        private string _uToken;

        public YotpoClientTests()
        {
            using (var s = new StreamReader(@"..\..\..\src\lib\ClientCredentials.json"))
            {
                var json = s.ReadToEnd();
                var creds = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                var clientId = creds["client_id"];
                var clientSecret = creds["client_secret"];
                _email = creds["email"];
                _client = new YotpoClient(clientId, clientSecret);
            }
        }

        [TestMethod]
        public void OAuthTokenReponseNotNull()
        {
            var response = _client.RequestOAuthToken();

            _uToken = response.AccessToken;

            Assert.IsNotNull(response.AccessToken);
        }

        [TestMethod]
        public void GetUserProfileResponseNotNull()
        {
            var response = _client.GetUserProfile(3);

            Assert.IsNotNull(response.Id);
        }

        [TestMethod]
        public void OrderSuccessfullySubmitted()
        {
            _uToken = _client.RequestOAuthToken().AccessToken;

            var products = new List<Product>();

            var sampleProduct = new Product
            {
                ProductSKU = "SKU124",
                ProductData = new ProductData()
                {
                    name = "Another Test Product",
                    description = "This is another test product.",
                    image = "http://fillmurray.com/200/300",
                    price = 22.88,
                    product_tags = "ugly, nasty, awesome",
                    url = "http://www.example.com"
                }
            };

            products.Add(sampleProduct);

            var order = new NewOrder
            {
                utoken = _uToken,
                customer_name = "Andrew Orr",
                email = "webmaster@example.com",
                order_date = DateTime.Now.ToString("yyyy-MM-dd"),
                order_id = "ORDERTEST1235",
                currency_iso = "USD",
                platform = "general",
                validate_data = true,
                user_id = "A12345",
                products = products
            };

            var responseCode = _client.CreateOrder(order);

            Assert.AreEqual(200, responseCode);
        }

        [TestMethod]
        public void OrdersRetrieved()
        {
            var responseCode = _client.GetOrders(_client.RequestOAuthToken().AccessToken);

            Assert.AreEqual(200, responseCode);
        }

        [TestMethod]
        public void Send_Test_Email_Is_Successful()
        {
            var responseCode = _client.SendTestEmail(_client.RequestOAuthToken().AccessToken, _email);

            Assert.AreEqual(200, responseCode);
        }

        [TestMethod]
        public void Deleted_Orders_Succesful()
        {
            var ordersToDelete = new OrdersToDelete
            {
                utoken = _client.RequestOAuthToken().AccessToken,
                orders = new[] { new OrderToDelete { order_id = "ORDER1234", skus = new[] { "SKU123" } } }
            };

            var responseCode = _client.DeleteOrders(ordersToDelete);

            Assert.AreEqual(200, responseCode);
        }

        [TestMethod]
        public void GetBottomLine_Successful()
        {
            var bottomLine = _client.GetBottomLine("1003");

            Console.WriteLine($"Score: {bottomLine.average_score} with {bottomLine.total_reviews} reviews.");

            Assert.IsTrue(bottomLine.total_reviews > 0);

            bottomLine = _client.GetBottomLine("9030");

            Assert.IsTrue(bottomLine.total_reviews == 0);
        }

        [TestMethod]
        public void GetReviewsForProduct_ReviewsReturnedForKnownReviewedProduct()
        {
            var productReviews = _client.GetReviewsForProduct("1003");

            Assert.IsTrue(productReviews.reviews.Length > 0);
        }

    }
}
