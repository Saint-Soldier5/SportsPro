using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SportsPro.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RegistrationController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private SportsProContext context { get; set; }
        public RegistrationViewModel viewModel;

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public RegistrationController(SportsProContext ctx)
        {
            context = ctx;
            viewModel = new RegistrationViewModel();
        }

        public ViewResult Index()
        {
            var data = new RegistrationViewModel()
            {
                Customer = new Customer { CustomerID = 0 }
            };

            IQueryable<Customer> query = context.Customers;

            data.Customers = query.ToList();
            return View(data);
        }

        [HttpPost]
        /*
         * store selected Customer in session state.
         */
        public IActionResult Index(RegistrationViewModel selectedCustomer)
        {
            if (selectedCustomer.Customer.CustomerID != 0)
            {

                var session = new MySession(HttpContext.Session);
                var sessionCustomer = session.GetCustomer();
                sessionCustomer = context.Customers.Find(selectedCustomer.Customer.CustomerID);
                session.SetCustomer(sessionCustomer);
            }
            else
            {
                TempData["msgDelete"] = "Please Choose a Customer!";
                return RedirectToAction("Index");
            }


            return RedirectToAction("RegProduct", "Registration");
        }

        public IActionResult RegProduct()
        {
            var data = new RegistrationViewModel()
            { CustomerProducts = new List<Product>(),
                Product = new Product { ProductID = 1 }};
            var session = new MySession(HttpContext.Session);
            var sessionCust = session.GetCustomer();
            //ViewBag.products = context.CustProds
            //    .Where(c => c.CustomerID == sessionCust.CustomerID)
            //    .Join(context.Products, cp => cp.ProductID,
            //    p => p.ProductID, (cp, p) => new Product()).ToList();
            ViewBag.products = context.Customers.Include("CustProds.Product").Where(cp=>cp.CustomerID==sessionCust.CustomerID
            ).Select(cp=>new Product()).ToList();
            // var customers = context.Customers.Include("CustProds.Product").ToList();

            // var products = context.Customers.Include("CustProds.Product").Where(c => c.CustomerID == sessionCust.CustomerID).ToList();
            Customer customer = context.Customers.Include("CustProds.Product").Where(c => c.CustomerID == sessionCust.CustomerID).Single();

            // List<Product> selectedProd=new List<Product>();
            IQueryable<Product> queryProducts = context.Products;
            
            foreach (var cusProduct in customer.CustProds)
                { data.CustomerProducts.Add(cusProduct.Product);
                queryProducts = queryProducts.Where(p => p.ProductID != cusProduct.ProductID);

            }
        


            data.Products = queryProducts.ToList();
            if (customer.CustProds.Count == 0)
            {
                TempData["message"] = $"{sessionCust.FullName} has no registered products.";
                //return RedirectToAction("RegProduct", "Registration");
            }

            //IQueryable<CustProd> query = context.CustProds;
            //query = query.Include(p => p.Product);

            /*
             *filters table by active technician i.e. technician stored in session (selected in dropbox 
             *on index action) and filters on incident dateclosed where dateclosed is not specified.
             */
            //query = query.Where(
            //    c => c.CustomerID == sessionCust.CustomerID);

            //data.Products = query.ToList();
            return View(data);
        }

        /*var invoices = invoiceList.Join(customerList,
         * i => i.CustomerID, c => c.CustomerID,
         * (i, c) => new { c.Name, i.InvoiceDate, i.InvoiceTotal })
         * .Where(ci => ci.InvoiceTotal > 150) .OrderBy(ci => ci.Name)
         * .ThenBy(ci => ci.InvoiceDate)
         * .Select(ci => new { ci.Name, ci.InvoiceTotal });*/

        [HttpPost]
        /*
         * store selected Customer in session state.
         */
        public IActionResult RegProduct(RegistrationViewModel selectedProduct)
        {
            var session = new MySession(HttpContext.Session);
            var sessionCust = session.GetCustomer();
            CustProd myNewCustProd = new CustProd()
            {
                CustomerID = sessionCust.CustomerID,
                ProductID = selectedProduct.Product.ProductID
            };
            TempData["msgAdd"] = $"{context.Products.Find(selectedProduct.Product.ProductID).Name} has been Registered to {sessionCust.FullName}.";
            context.CustProds.Add(myNewCustProd);
            context.SaveChanges();
            


            return RedirectToAction("RegProduct", "Registration");
        }

        [HttpGet]
        public IActionResult Delete(int id = 1)
        {
            var product = context.Products.Find(id);
            return View(product);
        }

        /*passes the Product object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the Customer from the database.
          Finally it redirects the user back to the Index action.*/
        [HttpPost]
        public IActionResult Delete(Product product)
        {

            var session = new MySession(HttpContext.Session);
            var sessionCust = session.GetCustomer();
            CustProd custProd = new CustProd()
            {
                CustomerID = sessionCust.CustomerID,
                ProductID = product.ProductID
            };
            TempData["msgDelete"] = $"{product.Name} has been Deleted from {sessionCust.FullName}.";
            context.CustProds.Remove(custProd);
            context.SaveChanges();
            
            return RedirectToAction("RegProduct", "Registration");
        }

    }
}
