using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Areas.Admin.Models
{
    public class CheckEmailTechnician
    {
        public static string EmailExists(IRepository<Technician> data, string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var existingTechnician = data.Get(new QueryOptions<Technician>
                {
                    Where = c => c.Email.ToLower() == email.ToLower()
                }
               );
                if (existingTechnician != null)
                {
                    return "Email already exists";
                }
            }
            return "";
        }
    }
}
