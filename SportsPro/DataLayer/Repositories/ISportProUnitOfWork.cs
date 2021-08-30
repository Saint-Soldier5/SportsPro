using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public interface ISportsProUnitOfWork
    {
        IRepository<Product> Products { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Technician> Technicians { get; }
        IRepository<Incident> Incidents { get; }

        //void DeleteCurrentBookAuthors(Book book);
        //void AddNewBookAuthors(Book book, int[] authorids);
        void Save();
    }
}
