using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Controllers
{
    [Area("Admin")]
    public class ValidationController : Controller
    {
     

        public JsonResult CheckEmailRemote(string emailAddress, [FromServices]IRepository<Customer> data)
        {
            string msg = CheckEmail.EmailExists(data, emailAddress);
            if (string.IsNullOrEmpty(msg))
            {
                TempData["okEmail"] = true;
                return Json(true);
            }
            else return Json(msg);
        }
    }
}
