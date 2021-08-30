using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SportsPro.Controllers
{
    [Area("Admin")]
    public class IncidentController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private ISportsProUnitOfWork data { get; set; }
        public IncidentController(ISportsProUnitOfWork unit) => data = unit;

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        

        //uses the context property to get a collection of Incident objects from the database.
        //Sorts the objects alphabetically by Incident Name.
        //Finally it passes the collection to the view.
        [Authorize(Roles = "Admin, Technician")]
        public IActionResult Index()
        {
            ViewBag.filter = Request.Query["filter"];
            string filter= Request.Query["filter"];
            var vm = new IncidentListViewModel()
            {
                MyFilter = filter
            };

            vm.Incidents = data.Incidents.List(new QueryOptions<Incident>
            { Includes = "Customer, Product, Technician",
                OrderBy = i => i.DateOpened
            });

            if (filter == "unassigned")
                vm.Incidents = data.Incidents.List(new QueryOptions<Incident>
                { Includes = "Customer, Product, Technician",
                    Where = i => i.TechnicianID == null,
                    OrderBy = i => i.DateOpened
                });
            
            if (filter == "open")
                vm.Incidents = data.Incidents.List(new QueryOptions<Incident>
                { Includes = "Customer, Product, Technician",
                    Where = i => i.DateClosed == null,
                    OrderBy = i => i.DateOpened
                });

            return View(vm);
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Incident/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Incident object to the view.
        Add() action passes an empty Incident object.
        Using ViewModel(IncidentViewModel) pass list of customers, products and techinicians
        to the view page.*/
        [Authorize(Roles = "Admin, Technician")]
        [HttpGet]
        public IActionResult Add()
        {
            var vm = new IncidentViewModel
            {
                Customers = data.Customers.List(new QueryOptions<Customer> {OrderBy = c => c.FirstName }),
                Products = data.Products.List(new QueryOptions<Product> { OrderBy = p => p.Name }),
                Technicians = data.Technicians.List(new QueryOptions<Technician> { OrderBy = t => t.Name }),
                DesiredAction = "Add",
                Incident = new Incident()

            };

            return View("Edit", vm);
        }

        /*the Edit() action passes a Incident object with data for an existing Incident by
                    passing the id parameter to the Find() method to retrieve a Incident from the
                    database.
        Using ViewModel(IncidentViewModel) pass list of customers, products and techinicians
        to the view page.*/
        [Authorize(Roles = "Admin, Technician")]
        [HttpGet]
        public IActionResult Edit(int id = 1)
        {
            var vm = new IncidentViewModel
            {
                Customers = data.Customers.List(new QueryOptions<Customer> { OrderBy = c => c.FirstName }),
                Products = data.Products.List(new QueryOptions<Product> { OrderBy = p => p.Name }),
                Technicians = data.Technicians.List(new QueryOptions<Technician> { OrderBy = t => t.Name }),
                DesiredAction = "Edit",
                Incident = data.Incidents.Get(id)

            };
            return View(vm);
        }

        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the IncidentID property of the Incident object.
          If the value is zero, it creates a new Incident passed into the Add() action.
            Otherwise, its an existing Incident, the code passes it to the Update().*/
        [Authorize(Roles = "Admin, Technician")]
        [HttpPost]
        public IActionResult Edit(IncidentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Incident.IncidentID == 0)
                {
                    TempData["msgAdd"] = $"{vm.Incident.Title} has been added.";
                    data.Incidents.Insert(vm.Incident);
                }
                else
                {
                    TempData["msgEdit"] = $"{vm.Incident.Title} has been edited.";
                    data.Incidents.Update(vm.Incident);
                }
                data.Save();
                return RedirectToAction("Index", "Incident");
            }
            else
            {
                vm.Customers = data.Customers.List(new QueryOptions<Customer> { OrderBy = c => c.FirstName });
                vm.Products = data.Products.List(new QueryOptions<Product> { OrderBy = p => p.Name });
                vm.Technicians = data.Technicians.List(new QueryOptions<Technician> { OrderBy = t => t.Name });
                vm.DesiredAction = (vm.Incident.IncidentID == 0) ? "Add" : "Edit";
                return View(vm);
            }
        }

        /*uses id parameter to retrieve a Incident object for the specified Incident from the
           database. Then passes the object to the view.*/
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id = 1)
        {
            var incident = data.Incidents.Get(id);
            return View(incident);
        }

        /*passes the Incident object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the Incident from the database.
          Finally it redirects the user back to the Index action.*/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            data.Incidents.Delete(incident);
            data.Save();
            TempData["msgDelete"] = $"{incident.Title} has been deleted.";
            return RedirectToAction("Index", "Incident");
        }
    }
}
