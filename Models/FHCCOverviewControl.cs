using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleTax
{
    public class FHCCOverviewControl
    {

        public string FhccBoxesSoldDaily { get; set; }

        public string FhccBoxesSoldMonthly { get; set; }

        [Display(Name = "From Date")]
        [Required(ErrorMessage = "Please enter date.")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        [Required(ErrorMessage = "Please enter date.")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ToDate { get; set; }

        public SearchDateViewModel  search { get; set; }


        //testing
    }
}
