using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SportsPro.Models.DataLayer.SeedData
{
    internal class SeedCustProds:IEntityTypeConfiguration<CustProd>
    {

        public void Configure(EntityTypeBuilder<CustProd> entity)
        {
            entity.HasData(
                      new CustProd { CustomerID = 1002, ProductID = 1 },
                   new CustProd { CustomerID = 1004, ProductID = 2 },
                   new CustProd { CustomerID = 1006, ProductID = 1 },
                   new CustProd { CustomerID = 1008, ProductID = 3 },
                   new CustProd { CustomerID = 1010, ProductID = 4 },
                   new CustProd { CustomerID = 1012, ProductID = 5 },
                   new CustProd { CustomerID = 1015, ProductID = 5 },
                   new CustProd { CustomerID = 1002, ProductID = 2 },
                   new CustProd { CustomerID = 1002, ProductID = 3 },
                   new CustProd { CustomerID = 1004, ProductID = 3 },
                   new CustProd { CustomerID = 1006, ProductID = 3 },
                   new CustProd { CustomerID = 1008, ProductID = 4 },
                   new CustProd { CustomerID = 1010, ProductID = 5 },
                   new CustProd { CustomerID = 1012, ProductID = 3 },
                   new CustProd
                   {
                       CustomerID = 1015,
                       ProductID = 4
                   }
              );
        }
    }
}
