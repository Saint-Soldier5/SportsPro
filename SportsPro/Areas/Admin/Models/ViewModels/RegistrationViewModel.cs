using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class RegistrationViewModel
    {
        
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> CustomerProducts { get; set; }

        
        public Customer Customer { get; set; }
        public Product Product { get; set; }

    }
}
