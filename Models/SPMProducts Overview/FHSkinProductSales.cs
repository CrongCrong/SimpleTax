using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace SimpleTax
{
    public class FHSkinProductSales
    {
        public FHSkinProducts Product { get; set; }

        public double ProductAmount { get; set; }
        public string ProductQuantity { get; set; }

        public bool IsLatest { get; set; }

        [BsonIgnore]
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public string ProductName { get; set; }

    }
}