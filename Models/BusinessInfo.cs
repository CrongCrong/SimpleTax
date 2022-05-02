

using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace SimpleTax
{
    public class BusinessInfo
    {
        [Display(Name = "Business name")]
        [Required(ErrorMessage = "Please enter registered business name.")]
        public string Businessname { get; set; }

        [Display(Name = "Business address")]
        [Required(ErrorMessage = "Please enter registered business address.")]
        public string Address { get; set; }

        [Display(Name = "TIN Number")]
        [Required(ErrorMessage = "Please enter Tin Number.")]
        public string TinNumber { get; set; }

        public bool IsDeleted { get; set; }

    }
}