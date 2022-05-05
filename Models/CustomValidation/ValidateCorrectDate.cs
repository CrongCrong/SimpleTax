using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleTax
{
    public class ValidateCorrectDate : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            DateTime dteOut;
            string t = DateTime.Parse(customer.Birthday.ToShortDateString()).ToShortDateString();

            try
            {
                DateTime sdt = DateTime.Parse(customer.Birthday.ToShortDateString());
            }
            catch (Exception e)
            {
                return new ValidationResult("Invalid Date. Please input correct date.");
            }

            if (DateTime.TryParse(t, out dteOut))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid Date. Please input correct date.");
            }

            //return !DateTime.TryParse(customer.BirthDate.ToShortDateString(), out dteOut) ? ValidationResult.Success :
            //    new ValidationResult("Invalid Date. Please input correct birth date.");

        }
    }
}