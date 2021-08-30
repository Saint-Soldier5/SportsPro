using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class IncidentViewModel
    {
        public Incident Incident { get; set; }

        public IEnumerable<Customer> Customers { get; set; }

        
        public IEnumerable<Product> Products { get; set; }

        
        public IEnumerable<Technician> Technicians { get; set; }
        public string DesiredAction { get; set; }
    }
}
