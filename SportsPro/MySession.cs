using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SportsPro.Models;

namespace SportsPro
{
    public class MySession
    {
        private ISession session { get; set; }
        public MySession(ISession sess)
        {
            session = sess;
        }

        //Technician
        private const string TechnicianKey = "technician";

        public Technician GetTechnician() =>
            session.GetObject<Technician>(TechnicianKey) ?? new Technician();

        public void SetTechnician(Technician technician) =>
            session.SetObject(TechnicianKey, technician);

        //Customer
        private const string CustomerKey = "customer";

        public Customer GetCustomer() =>
            session.GetObject<Customer>(CustomerKey) ?? new Customer();

        public void SetCustomer(Customer customer) =>
            session.SetObject(CustomerKey, customer);
    }
}
