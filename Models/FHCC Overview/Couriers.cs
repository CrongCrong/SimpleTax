using System;
using MongoDB.Bson;

namespace SimpleTax
{
    public class Couriers
    {
        public Couriers()
        {
        }

        public ObjectId Id { get; set; }

        public string CourierName { get; set; }

        public string Description { get; set; }

        public bool isDeleted { get; set; }
    }
}
