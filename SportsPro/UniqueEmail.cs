using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;

namespace SportsPro
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        private SportsProContext context { get; set; }

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public UniqueEmailAttribute(SportsProContext ctx)
        {
            context = ctx;
        }
        protected override ValidationResult IsValid(object value, ValidationContext ctx)
            {

            if (value is string)
            {
                string emailToCheck = value.ToString();
                List<string> allEmails = context.Customers.Select(c => c.Email).ToList();

                if (allEmails.FirstOrDefault(e => e == emailToCheck) is null)
                {
                    return ValidationResult.Success;
                }
            }
                //foreach (string email in allEmails)
                //{ 
                //if (email==emailToCheck)
                   
                //}
                string msg = base.ErrorMessage ?? $"{ctx.DisplayName} must be a unique email."; 
                return new ValidationResult(msg);
            }
        

    }
}
