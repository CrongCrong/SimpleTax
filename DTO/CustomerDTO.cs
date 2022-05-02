

using MongoDB.Bson;
using System;

namespace SimpleTax
{
    public class CustomerDTO
    {
        public ObjectId Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Businessname { get; set; }
        public string Address { get; set; }
        public string TinNumber { get; set; }
        public bool IsDeleted { get; set; }
        public string StrFullName { get; set; }
    }
}