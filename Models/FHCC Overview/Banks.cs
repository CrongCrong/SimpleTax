
using MongoDB.Bson;

namespace SimpleTax
{
    public class Banks
    {
        public Banks()
        {
        }

        public ObjectId Id { get; set; }

        public string BankName { get; set; }

        public string Description { get; set; }

        public bool isDeleted { get; set; }
    }
}
