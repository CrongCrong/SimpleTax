using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SimpleTax
{
    public class User
    {
        public ObjectId Id { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter username.")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter correct password.")]
        [BsonIgnore]
        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public bool IsAdmin { get; set; }

        public byte[] bHash { get; set; }

        public byte[] bSalt { get; set; }

        public bool IsDeleted { get; set; }

        [BsonIgnore]
        public bool IsValidCredentials { get; set; }


        [BsonIgnore]
        //[Display(Name = "Confirm Password")]
        //[Required(ErrorMessage = "Password did not match.")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}