using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class IncidentListViewModel:IncidentViewModel
    {
        public IEnumerable<Incident> Incidents { get; set; }
        public string MyFilter { get; set; }
       

    }
}
