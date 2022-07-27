using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleTax
{
    public class SPMProductsOverviewControl
    {
        public SPMProductsOverviewControl()
        {
        }

        public string Real50gSold { get; set; }

        public string Real350gSold { get; set; }

        public string Real1KgSold { get; set; }

        public List<FHSkinProductSales> ListProductsSold { get; set; }

        public FHSkinProductSales products { get; set; }

        [Display(Name = "From Date")]
        [Required(ErrorMessage = "Please enter date.")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        [Required(ErrorMessage = "Please enter date.")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ToDate { get; set; }

        public SearchDateViewModel search { get; set; }

    }
}
