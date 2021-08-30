using System;
using System.Collections.Generic;
using System.Text;
using SportsPro.Models;

namespace SportsProTest.FakeClassesNotUsed
{
    public class FakeProductRepository : IRepository<Product>
    {
        public int Count => throw new NotImplementedException();

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public Product Get(QueryOptions<Product> options)
        {
            return new Product();
        }

        public Product Get(int id)
        {
            throw new NotImplementedException();
        }

        public Product Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Product entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> List(QueryOptions<Product> options)
        {
            return new List<Product>();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
