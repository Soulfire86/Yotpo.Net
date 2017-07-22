# Yotpo.Net
.NET client library for Yotpo Reviews Platform API

![Bulid Status](https://ci.appveyor.com/api/projects/status/k0fctg055sy8l826?svg=true "Build Status")

## Methods Implemented
- **Authentication**
  - Request OAuth Token
- **Purchases**
  - Create an Order
  - Delete a Purchase
- **Users**
  - Create a New User
  - Retrieve User Profile Data
- **Mail After Purchase**
  - Send Test Email

## Usage

**Example:**

    using YotpoNet;
    using YotpoNet.Models;

    var yotpoClient = new YotpoClient("Your API Key", "Your API secret");
    var userToken = yotpoClient.RequestOAuthToken();

    yotpoClient.CreateOrder(new NewOrder
    {
        validate_data = true,
        utoken = userToken.AccessToken,
        email = "customer@email.com",
        customer_name = "Customer Name",
        order_id = "376611",
        order_date = "2017-07-22",
        currency_iso = "USD",
        products = new List<Product>
        {
            new Product
            {
                ProductSKU = "SKU12300",
                ProductData = new ProductData
                {
                    url = "http://example_product_url1.com",
                    name = "product1",
                    image = "http://images2.fanpop.com/image/photos/13300000/A1.jpg",
                    description = "this is the description of a product",
                    price = 100
                }
            }
        }
    });

### Client Secrets
Update src/lib/ClientCredentials.json with your client key and secret for Unit Testing.
