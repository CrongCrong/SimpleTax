

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SimpleTax
{
    public class FHSkinSales
    {

        public ObjectId Id { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TransactionDate { get; set; }
        public double TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string DRNumber { get; set; }
        public double TotalBalance { get; set; }

        public List<FHSkinProducts> ProductsBought { get; set; }
        public List<FHSkinProductSales> ProductsSales { get; set; }

        public FHSkinClient Client { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsWithBalance { get; set; }
        public bool IsForVictor { get; set; }
        public bool IsOfficial { get; set; }

        [BsonIgnore]
        public string StrTransactionDate { get; set; }
    }
}