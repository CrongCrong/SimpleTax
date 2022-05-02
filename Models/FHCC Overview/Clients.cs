using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleTax
{
    public class Clients
    {
        public Clients()
        {
        }

        public ObjectId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactNo { get; set; }

        public string Address { get; set; }

        public string EmailAddress { get; set; }

        public bool isDeleted { get; set; }

        public bool isWithShopee { get; set; }

        [BsonIgnore]
        public string strFullName { get; set; }
    }
}
