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
    [Area("Technician")]
    [Authorize(Roles = "Admin, Technician")]
    public class TechIncidentController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private SportsProContext context { get; set; }
        public TechnicianListViewModel viewModel;

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public TechIncidentController(SportsProContext ctx)
        {
            context = ctx;
            viewModel = new TechnicianListViewModel();
        }

        //uses the context property to get a collection of Technican objects from the database.
        //Passes the collection to the view.
        public IActionResult Index()
        {

            var data = new TechnicianListViewModel()
            { Technician = new Technician { TechnicianID = 1 } };
           

            IQueryable<Technician> query = context.Technicians;
            

            data.Technicians = query.ToList();
            return View(data);
        }

        [HttpPost]
        /*
         * store selected technician in session state.
         */
        public IActionResult Index(TechnicianListViewModel selectedTechnician)
        {
            var session = new MySession(HttpContext.Session);
            var sessionTech = session.GetTechnician();
            sessionTech = context.Technicians.Find(selectedTechnician.Technician.TechnicianID);
            session.SetTechnician(sessionTech);
         

            return RedirectToAction("Success", "TechIncident");
        }

        /*
         * query (IQueryable) joins Incidents table with Product and Technician tables and displays the 
         * resulting properties(columns)
         */
        public IActionResult Success()
        {

            var data = new IncidentListViewModel();
            var session = new MySession(HttpContext.Session);
            var sessionTech = session.GetTechnician();


            IQueryable<Incident> query = context.Incidents;
            query = query.Include(c => c.Customer)
                .Include(p => p.Product)
                .Include(t => t.Technician)
                .OrderBy(i => i.DateOpened);

            /*
             *filters table by active technician i.e. technician stored in session (selected in dropbox 
             *on index action) and filters on incident dateclosed where dateclosed is not specified.
             */
            query = query.Where(
                i => i.TechnicianID == sessionTech.TechnicianID)
                .Where(
                i => i.DateClosed == null);
            
            data.Incidents = query.ToList();
            /*checking for open incidents using an if statement and displaying a message if there 
             * are no open incidents. Returns the user to the index action method (in TechIncident controller).
             */
            if (data.Incidents.Count() == 0)
            {
                TempData["message"] = $"{sessionTech.Name} has no open incidents.";
                return RedirectToAction("Index", "TechIncident"); }
            return View(data);
       
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Incident/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Incident object to the view.
        Add() action passes an empty Incident object.*/
        [HttpGet]
        public IActionResult Add()
        {
            var data = new IncidentViewModel
            {
                Customers = context.Customers.OrderBy(c => c.FirstName).ToList(),
                Products = context.Products.OrderBy(p => p.Name).ToList(),
                Technicians = context.Technicians.OrderBy(t => t.Name).ToList(),
                DesiredAction = "Add",
                Incident = new Incident()

            };

            return View("Edit", data);
        }

        /*the Edit() action passes a Incident object with data for an existing Incident by
                    passing the id parameter to the Find() method to retrieve a Incident from the
                    database.*/
        [HttpGet]
        public IActionResult Edit(int id = 1)
        {
            var data = new IncidentViewModel
            {
                Customers = context.Customers.OrderBy(c => c.FirstName).ToList(),
                Products = context.Products.OrderBy(p => p.Name).ToList(),
                Technicians = context.Technicians.OrderBy(t => t.Name).ToList(),
                DesiredAction = "Edit",
                Incident = context.Incidents.Find(id)

            };
            return View(data);
        }

        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the IncidentID property of the Incident object.
          If the value is zero, it creates a new Incident passed into the Add() action.
            Otherwise, its an existing Incident, the code passes it to the Update().*/
        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            if (ModelState.IsValid)
            {
                if (incident.IncidentID == 0)
                {
                    
                    context.Incidents.Add(incident);
                }
                else
                {
                    context.Incidents.Update(incident);
                }
                context.SaveChanges();
                return RedirectToAction("Index", "Incident");
            }
            else
            {
                ViewBag.Action = (incident.IncidentID == 0) ? "Add" : "Edit";
                return View(incident);
            }
        }

        /*uses id parameter to retrieve a Incident object for the specified Incident from the
           database. Then passes the object to the view.*/
        [HttpGet]
        public IActionResult Delete(int id = 1)
        {
            var incident = context.Incidents.Find(id);
            return View(incident);
        }

        /*passes the Incident object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the Incident from the database.
          Finally it redirects the user back to the Index action.*/
        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("Index", "Incident");
        }
    }
}
