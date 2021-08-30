using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Areas.Admin.Models
{
    public static class CheckEmail
    {
        public static string EmailExists(IRepository<Customer> data, string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var existingCustomer = data.Get(new QueryOptions<Customer>
                {
                    Where = c => c.Email.ToLower() == email.ToLower()
                }
               );
                if (existingCustomer != null)
                {
                    return "Email already exists";
                }
            }
            return "";
        }
    }
}
