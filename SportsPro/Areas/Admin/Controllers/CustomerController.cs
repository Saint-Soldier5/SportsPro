using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using SportsPro.Areas.Admin.Models;
//Using directive for the EF Core namespace. See next comment.

namespace SportsPro.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private SportsProContext context { get; set; }
        private Repository<Customer> data { get; set; }

        private Repository<Country> countryData { get; set; }


        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public CustomerController(SportsProContext ctx)
        {
            data = new Repository<Customer>(ctx);
            countryData = new Repository<Country>(ctx);
            context = ctx;
        }

        //uses the context property to get a collection of Customer objects from the database.
        //Sorts the objects alphabetically by Customer Name.
        //Finally it passes the collection to the view.
        //Index() uses the Include() method to select the Country data related to each Customer.
        public ViewResult Index()
        {
            var customers = data.List(new QueryOptions<Customer>
            {
                Includes = "Country",
                OrderBy = c => c.FirstName
            });
                
            return View(customers);
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Customer/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Customer object to the view.
        Add() action passes an empty Customer object.*/
        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Countries = countryData.List(new QueryOptions<Country> 
            { OrderBy = c => c.Name });
            ViewBag.Action = "Add";
            return View("Edit", new Customer());
        }

        /*the Edit() action passes a Customer object with data for an existing Customer by
            passing the id parameter to the Find() method to retrieve a Customer from the
            database.*/
        [HttpGet]
        public ViewResult Edit(int id = 1002)
        {
            ViewBag.Countries = countryData.List(new QueryOptions<Country>
            { OrderBy = c => c.Name });
            ViewBag.Action = "Edit";
            var customer = data.Get(id);
            return View(customer);
        }
       
        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the CustomerID property of the Customer object.
          If the value is zero, it creates a new Customer passed into the Add() action.
            Otherwise, its an existing Customer, the code passes it to the Update().*/
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (customer.CustomerID == 0)
            {
                string EmailToCheck = nameof(customer.Email);

                string message = CheckEmail.EmailExists(data, customer.Email);
                if (message != "")
                {
                    ModelState.AddModelError(
                      EmailToCheck, message);
                }
            }

            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                {
                    TempData["msgAdd"] = $"{customer.FullName} has been added.";
                    data.Insert(customer);
                }
                else
                {
                    TempData["msgEdit"] = $"{customer.FullName} has been edited.";
                    data.Update(customer);
                }
                data.Save();
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                ViewBag.Action = (customer.CustomerID == 0) ? "Add" : "Edit";
                ViewBag.Countries = countryData.List(new QueryOptions<Country>
                { OrderBy = c => c.Name }); 
                return View(customer);
            }
        }

        /*uses id parameter to retrieve a Customer object for the specified Customer from the
           database. Then passes the object to the view.*/
        [HttpGet]
        public ViewResult Delete(int id = 1002)
        {
            Customer customer = data.Get(id);
            return View(customer);
        }

        /*passes the Customer object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the Customer from the database.
          Finally it redirects the user back to the Index action.*/
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            
            data.Delete(customer);
            data.Save();
            TempData["msgDelete"] = $"{customer.FirstName} {customer.LastName} has been deleted.";
            return RedirectToAction("Index", "Customer");
        }
    }
}
