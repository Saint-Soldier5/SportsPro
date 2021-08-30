using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.AspNetCore.Authorization;

namespace SportsPro.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
       // private SportsProContext context { get; set; }
        private IRepository<Product> data { get; set; }

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public ProductController(IRepository<Product> rep) => data = rep;
       

        //uses the context property to get a collection of Product objects from the database.
        //Sorts the objects alphabetically by Product Name.
        //Finally it passes the collection to the view.
        public ViewResult Index()
        {
            var products = data.List(new QueryOptions<Product>
            { OrderBy = p => p.ProductCode });
            return View(products);
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Product/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Product object to the view.
        Add() action passes an empty Product object.*/
        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Product());
        }

        /*the Edit() action passes a Product object with data for an existing product by
            passing the id parameter to the Find() method to retrieve a product from the
            database.*/
        [HttpGet]
        public ViewResult Edit(int id = 1)
        {
            ViewBag.Action = "Edit";
            var product = data.Get(id);
            
            return View(product);
        }

        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the ProductID property of the Product object.
          If the value is zero, it creates a new product passed into the Add() action.
            Otherwise, its an existing product, the code passes it to the Update().*/
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                {
                    TempData["msgAdd"] = $"{product.Name} has been added.";
                    data.Insert(product);
                }   
                else
                {
                    TempData["msgEdit"] = $"{product.Name} has been edited.";
                    data.Update(product);
                }
                data.Save();
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
                return View(product);
            }
        }

        /*uses id parameter to retrieve a Product object for the specified product from the
           database. Then passes the object to the view.*/
        [HttpGet]
        public ViewResult Delete(int id = 1)
        {
            var product = data.Get(id);
            return View(product);
        }

        /*passes the Product object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the product from the database.
          Finally it redirects the user back to the Index action.*/
        [HttpPost]
        public RedirectToActionResult Delete(Product product)
        {
            data.Delete(product);
            data.Save();
            TempData["msgDelete"] = $"{product.Name} has been deleted.";
            return RedirectToAction("Index", "Product");
        }
    }
}
