using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.DataLayer.Repositories
{
    public class SportsProUnitOfWork : ISportsProUnitOfWork
    {
        private SportsProContext context { get; set; }
        public SportsProUnitOfWork(SportsProContext ctx) => context = ctx;

        private IRepository<Product> productData;
        public IRepository<Product> Products
        {
            get
            {
                if (productData == null)
                    productData = new Repository<Product>(context);
                return productData;
            }
        }

        private IRepository<Customer> customerData;
        public IRepository<Customer> Customers
        {
            get
            {
                if (customerData == null)
                    customerData = new Repository<Customer>(context);
                return customerData;
            }
        }

        private IRepository<Technician> technicianData;
        public IRepository<Technician> Technicians
        {
            get
            {
                if (technicianData == null)
                    technicianData = new Repository<Technician>(context);
                return technicianData;
            }
        }

        private IRepository<Incident> incidentData;
        public IRepository<Incident> Incidents
        {
            get
            {
                if (incidentData == null)
                    incidentData = new Repository<Incident>(context);
                return incidentData;
            }
        }

        public void Save() => context.SaveChanges();
    }
}
