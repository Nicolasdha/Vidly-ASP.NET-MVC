using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Min18YearsIfMember : ValidationAttribute 
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            /* 
                Need to check selected membership type - that is where the ValidationContex comes in using validationContext.ObjectInstance which gives us access to the
            containing class which is - Customer. B/c this is an object we need to cast it to customer with var customer = (Customers)validationContext.ObjectInstance

            Now to check the selected membership type is the correct type
            return ValidationResult.Success; = successful validation result

            return new ValidationResult("bday req") - For all errors 

                */
            var customer = (Customers)validationContext.ObjectInstance;
            if(customer.MembershipTypeId == 1)
            {
                return ValidationResult.Success;
            }

            if (customer.Birthdate == null)
                return new ValidationResult("Birthdate is required");

            var age = DateTime.Today.Year - customer.Birthdate.Year;

            return (age >= 18)
                ? ValidationResult.Success
                : new ValidationResult("Customer should be at least 18 years old");

        }
    }
}
