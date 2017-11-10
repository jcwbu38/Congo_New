using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test5.Models;

namespace Congo.Controllers
{
    public class NewProductController : Controller
    {
        // GET: NewProduct
        public ActionResult Index()
        {
            return View();
        }
    }
}