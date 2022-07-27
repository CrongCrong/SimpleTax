using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleTax
{
    public class SimpleMembers
    {
        public ObjectId Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
    }
}