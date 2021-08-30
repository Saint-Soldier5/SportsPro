using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class TechnicianListViewModel
    {
        public List<Technician> Technicians { get; set; }
        public Technician Technician { get; set; }
        public string MyFilter { get; set; }
    }
}
