using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.AspNetCore.Authorization;
using SportsPro.Areas.Admin.Models;

namespace SportsPro.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TechnicianController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private IRepository<Technician> data { get; set; }

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public TechnicianController(IRepository<Technician> rep) => data = rep;

        //uses the context property to get a collection of Technician objects from the database.
        //Sorts the objects alphabetically by Technician Name.
        //Finally it passes the collection to the view.
        public IActionResult Index()
        {
            var technicians = data.List(new QueryOptions<Technician>
            {
                OrderBy = t => t.TechnicianID
            });

            return View(technicians);
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Technician/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Technician object to the view.
        Add() action passes an empty Technician object.*/
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Technician());
        }

        /*the Edit() action passes a Technician object with data for an existing Technician by
            passing the id parameter to the Find() method to retrieve a Technician from the
            database.*/
        [HttpGet]
        public IActionResult Edit(int id = 11)
        {
            ViewBag.Action = "Edit";
            var technician = data.Get(id);
            return View(technician);
        }

        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the TechnicianID property of the Technician object.
          If the value is zero, it creates a new Technician passed into the Add() action.
            Otherwise, its an existing Technician, the code passes it to the Update().*/
        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            if (technician.TechnicianID == 0)
            {
                string EmailToCheck = nameof(technician.Email);


                //if (allEmails.FirstOrDefault(e => e == customer.Email))
                string message = CheckEmailTechnician.EmailExists(data, technician.Email);
                if (message != "")
                {
                    ModelState.AddModelError(
                      EmailToCheck, message);
                }
            }
            if (ModelState.IsValid)
            {
                if (technician.TechnicianID == 0)
                {
                    TempData["msgAdd"] = $"{technician.Name} has been added.";
                    data.Insert(technician);
                }
                else
                {
                    TempData["msgEdit"] = $"{technician.Name} has been edited.";
                    data.Update(technician);
                }
                data.Save();
                return RedirectToAction("Index", "Technician");
            }
            else
            {
                ViewBag.Action = (technician.TechnicianID == 0) ? "Add" : "Edit";
                return View(technician);
            }
        }

        /*uses id parameter to retrieve a Technician object for the specified Technician from the
          database. Then passes the object to the view.*/
        [HttpGet]
        public IActionResult Delete(int id = 11)
        {
            var technician = data.Get(id);
            return View(technician);
        }

        /*passes the Technician object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the Technician from the database.
          Finally it redirects the user back to the Index action.*/
        [HttpPost]
        public IActionResult Delete(Technician technician)
        {
            data.Delete(technician);
            data.Save();
            TempData["msgDelete"] = $"{technician.Name} has been deleted.";
            return RedirectToAction("Index", "Technician");
        }
    }
}
