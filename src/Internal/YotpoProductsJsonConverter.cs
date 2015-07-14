using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YotpoNet.Models;

namespace YotpoNet.Internal
{
    public class YotpoProductsJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true; //WTF, I dunno...guess this would be true for the one case I wanna use it.
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var products = (List<Product>)value;

            foreach (var product in products)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(product.ProductSKU);
                writer.WriteRawValue(JsonConvert.SerializeObject(product.ProductData));

                writer.WriteEndObject();
            }

            writer.Flush();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException("Don't need this shit! Didn't implement it.");
        }
    }
}
