using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SportsPro.Models
{
    public class User : IdentityUser 
    {
        //Inherits all IdentityUser properties
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [NotMapped]
        public IList<string> RoleNames { get; set; }
    }
}
