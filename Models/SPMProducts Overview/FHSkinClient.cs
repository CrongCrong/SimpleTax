

using MongoDB.Bson;

namespace SimpleTax
{
    public class FHSkinClient
    {

        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string TIN { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
    }
}