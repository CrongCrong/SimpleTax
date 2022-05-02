using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleTax
{
    public class Customer
    {
        public ObjectId Id { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please enter first name.")]
        public string Firstname { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Please enter last name.")]
        public string Lastname { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please enter birth date.")]
        [ValidateCorrectDate]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Business name")]
        [Required(ErrorMessage = "Please enter registered business name.")]
        public string Businessname { get; set; }

        [Display(Name = "Business address")]
        [Required(ErrorMessage = "Please enter registered business address.")]
        public string Address { get; set; }

        [Display(Name = "TIN Number")]
        [Required(ErrorMessage = "Please enter Tin Number.")]
        public string TinNumber { get; set; }

        [Display(Name = "Line of business")]
        [Required(ErrorMessage = "Please enter line of business.")]
        public string LineOfBusiness { get; set; }

        [Display(Name = "Registration Date")]
        [Required(ErrorMessage = "Please enter registration date.")]
        [ValidateCorrectDate]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Cellphone")]
        public string Cellphone { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public bool IsDeleted { get; set; }

        public User Credentials { get; set; }

        public List<Customer> Referrals { get; set; }

        [Display(Name = "Tax Payer")]
        public TaxPayerType TaxPayer;

        [BsonIgnore]
        public string StrFullName { get; set; }

        [BsonIgnore]
        public string StrCustomerId { get; set; }

        [BsonIgnore]
        [Display(Name = "Tax Payer")]
        public string StrTaxpayerType { get; set; }


    }
}