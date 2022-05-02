using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleTax
{
    public class Products
    {
        public Products()
        {
        }

        public ObjectId Id { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool isDeleted { get; set; }

        [BsonIgnore]
        public double Qty { get; set; }

        [BsonIgnore]
        public double StocksOnHand { get; set; }

        [BsonIgnore]
        public string StrProductName { get; set; }
    }
}
