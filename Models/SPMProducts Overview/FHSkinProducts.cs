using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleTax
{
    public class FHSkinProducts
    {
        public ObjectId Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public bool IsDeleted { get; set; }

        public double ProductAmount { get; set; }
        public string ProductQuantity { get; set; }

    }
}