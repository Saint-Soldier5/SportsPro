using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.ViewComponents
{
    public class CustomerDropDown : ViewComponent
    {
        private IRepository<Customer> data { get; set; }
        public CustomerDropDown(IRepository<Customer> rep) => data = rep;

        public IViewComponentResult Invoke(string Value, string DefaultText, string DefaultValue)
        {
            var customer = data.List(new QueryOptions<Customer>
            {
                OrderBy = c => c.LastName
            });

            var vm = new DropDownViewModel
            {
                SelectedValue = Value,
                DefaultValue = DefaultValue,
                DefaultText = DefaultText,
                Items = customer.ToDictionary(
                    c => c.CustomerID.ToString(), c => c.FullName)
            };

            return View("~/Views/Shared/Components/Dropdown.cshtml", vm);
        }
    }
}
