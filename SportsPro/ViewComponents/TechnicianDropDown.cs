using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.ViewComponents
{
    public class TechnicianDropDown : ViewComponent
    {
        private IRepository<Technician> data { get; set; }
        public TechnicianDropDown(IRepository<Technician> rep) => data = rep;

        public IViewComponentResult Invoke(string Value, string DefaultText, string DefaultValue)
        {
            var countries = data.List(new QueryOptions<Technician>
            {
                OrderBy = c => c.Name
            });

            var vm = new DropDownViewModel
            {
                SelectedValue = Value,
                DefaultValue = DefaultValue,
                DefaultText = DefaultText,
                Items = countries.ToDictionary(
                    c => c.TechnicianID.ToString(), c => c.Name)
            };

            return View("~/Views/Shared/Components/Dropdown.cshtml", vm);
        }
    }
}
